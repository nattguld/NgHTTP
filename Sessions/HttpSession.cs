using NgHTTP.Browser;
using NgHTTP.Configs;
using NgHTTP.Content;
using NgHTTP.Proxies;
using NgHTTP.Requests;
using NgHTTP.Requests.Impl;
using NgHTTP.Requests.Responses;
using NgHTTP.Requests.Responses.Bodies;
using NgHTTP.Requests.Responses.Bodies.Impl;
using NgHTTP.Requests.Responses.Decoders.Impl;
using NgHTTP.Requests.Security;
using NgHTTP.Sockets.Http;
using NgHTTP.Storage;
using NgHTTP.Storage.Applications.Impl;
using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil;
using NgUtil.Debugging.Logging;
using NgUtil.Files;
using NgUtil.Generics.Kvps.Impl;
using NgUtil.Maths;
using NgUtil.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NgHTTP.Sessions {

    public abstract class HttpSession<TA> : IDisposable where TA : Application {

        public const int MaxRedirects = 5;

        public const int MaxConnectionAttempts = 3;

        public string Uuid { get; }

        public HttpSessionPolicy SessionPolicy { get; set; }

        public List<TA> Apps { get; }

        public List<string> AccessedHosts { get; }

        public List<Request> RedirectionChain { get; }

        public BrowserConfig BrowserCfg { get; set; }

        public static Proxy Proxy { get; private set; }

        public static NetworkType NetworkType { get; set; }

        public bool AutoRedirectDisabled { get; set; }

        public bool IsPersistent { get; set; }

        public TA PreviousApp { get; private set; }


        public HttpSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy) {
            Uuid = MathUtil.GetUniqueId();
            Apps = new List<TA>(); ;
            AccessedHosts = new List<string>();
            RedirectionChain = new List<Request>();

            SessionPolicy = sessionPolicy;
            BrowserCfg = browserCfg;
            SetProxy(proxy);
        }

        public HttpSession<TA> SetProxy(Proxy proxy) {
            Proxy = proxy;
            NetworkType = proxy is null ? NetworkType.Residential : proxy.NetworkType;
            return this;
        }

        protected abstract TA InstantiateApp(string domain);

        public void Dispose() {
            if (SessionPolicy != HttpSessionPolicy.Default) {
                ClearData();
            }
        }

        protected abstract void BuildRequestHeaders(StringStringKeyValuePairContainer reqHeadersContainer, Request req, TA app, bool ssl, bool redirect);

        public RequestResponse DispatchRequest(Request request) {
            return DispatchRequest(request, false);
        }

        public RequestResponse DispatchRequest(Request request, bool newTab) {
            string host = HttpUtil.GetDomain(request.Url);
            return DispatchRequest(request, newTab, HttpSocketFactory.SslHosts.Contains(host));
        }

        public RequestResponse DispatchRequest(Request request, bool sideSession, bool ssl) {
            request.Attempts++;

            if ((++request.Attempts) > MaxConnectionAttempts) {
                RequestResponse tooManyAttemptsResponse = new RequestResponse(request.Url, request.ExpectedResponseCode
                    , new ResponseStatus(HttpCode.Unknown, "Too many failed attempts")
                    , new StringResponseBody("Failed to dispatch request (attempts: " + (request.Attempts - 1) + ")"), null);
                request.Attempts = 0;
                return tooManyAttemptsResponse;
            }
            TA app = GetAppByUrl(request.Url);

            if (app.GetType() == typeof(WebApplication)) {
                WebApplication webApp = app as WebApplication;

                if (PreviousApp is null || sideSession) {
                    webApp.LastNavigatedUrl = null;

                } else if (PreviousApp != null && PreviousApp.GetType() == typeof(WebApplication) && !sideSession) {
                    WebApplication prevWebApp = PreviousApp as WebApplication;
                    webApp.LastNavigatedUrl = prevWebApp.LastNavigatedUrl;
                }
            }
            string endpoint = request.Url;

            //if (Proxy is null) {
                endpoint = StringFunc.Substring(request.Url, request.Url.IndexOf(app.Domain) + app.Domain.Length, request.Url.Length);

                if (endpoint == string.Empty) {
                    endpoint = "/";
                }
            //}
            //int endpointLength = request.Url.Length - app.Domain.Length - request.Url.IndexOf(app.Domain);
            //string endpoint = endpointLength == 0 ? "/" : StringFunc.Substring(request.Url, endpointLength, request.Url.Length);

            StringStringKeyValuePairContainer reqHeadersContainer = new StringStringKeyValuePairContainer();
            BuildRequestHeaders(reqHeadersContainer, request, app, ssl, RedirectionChain.Count > 0);

            if (HttpConfig.DebugMode) {
                Console.WriteLine("Host: " + app.Domain + ", Endpoint: " + endpoint + ", SSL: " + ssl);
            }
            if (request.CustomHeaders.Kvps.Count > 0) {
                foreach (StringStringKeyValuePair kvp in request.CustomHeaders.Kvps) {
                    if (kvp.Value is null) {
                        reqHeadersContainer.Remove(kvp.Key);
                        continue;
                    }
                    reqHeadersContainer.Put(kvp);
                }
            }
            RequestResponse rr;

            try {
                using (HttpSocket httpSocket = HttpSocketFactory.Connect(Proxy, app.Domain, request.Port, BrowserCfg, ssl)) {
                    if (httpSocket is null) {
                        return new RequestResponse(request.Url, request.ExpectedResponseCode
                            , new ResponseStatus(HttpCode.Unknown, "Failed to open socket for " + app.Domain + ":" + request.Port)
                            , new StringResponseBody("Failed to open socket for " + app.Domain + ":" + request.Port), null);
                    }
                    httpSocket.WriteLine(request.RequestType.Notation + " " + endpoint + " " + BrowserCfg.HttpVersion.Notation);

                    foreach (StringStringKeyValuePair kvp in reqHeadersContainer.Kvps) {
                        httpSocket.WriteLine(kvp.Key + ": " + kvp.Value);
                    }
                    httpSocket.WriteLine();
                    httpSocket.FlushUnderlying();

                    if (request.HasBody()) {
                        ContentRequest contReq = (ContentRequest)request;
                        ContentBody contBody = contReq.ContentBody;

                        if (contBody != null) {
                            contBody.Write(httpSocket);
                        }
                    }
                    httpSocket.FlushUnderlying();

                    HttpHeaderDecoder headerDecoder = new HttpHeaderDecoder();
                    headerDecoder.Decode(httpSocket.Stream);

                    ResponseStatus respStatus = headerDecoder.ResponseStatus;
                    StringStringKeyValuePairContainer respHeadersContainer = headerDecoder.Headers;

                    if (respStatus.HttpCode == HttpCode.Invalid) {
                        Console.WriteLine("Failed to decode headers (" + request.RequestType.NameIdentifier + " => " + request.Url + ")");
                        return DispatchRequest(request, sideSession, ssl);
                    }
                    if (SessionPolicy != HttpSessionPolicy.NoData && app.GetType() == typeof(WebApplication)) {
                        WebApplication webApp = app as WebApplication;
                        webApp.CookieJar.ImportCookies(headerDecoder.Cookies);
                    }
                    IBaseResponseBody respBody = respStatus.HttpCode != HttpCode.NoContent
                        ? ResponseBodyParser.ParseResponseBody(request, respHeadersContainer, httpSocket.Stream)
                        : new StringResponseBody("");

                    rr = new RequestResponse(request.Url, request.ExpectedResponseCode, respStatus, respBody, respHeadersContainer);
                }
            } catch (SocketException ex) {
                new Logger("alo").Exception(ex, false);
                return DispatchRequest(request, sideSession, ssl);

            } catch (Exception ex) {
                Console.WriteLine("EXCEPTION " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                return DispatchRequest(request, sideSession, ssl);
            }
            if (!AccessedHosts.Contains(app.Domain)) {
                AccessedHosts.Add(app.Domain);
            }
            if (!rr.Validate()) {
                if (rr.ResponseStatus.HttpCode.IsRedirection() && !AutoRedirectDisabled) {
                    return FollowRedirects(request, rr, sideSession, ssl);

                } else if (rr.ResponseStatus.HttpCode.IsClientError()) {
                    if (rr.ResponseStatus.HttpCode == HttpCode.NotFound
                        && !request.Url.EndsWith(Delimiters.Slash, StringComparison.Ordinal)) {
                        Console.WriteLine("Request returns 404 (Not Found) while url is missing a trailing slash (/). Maybe you need one?");

                    } else if ((rr.ResponseStatus.HttpCode == HttpCode.Forbidden || rr.ResponseStatus.HttpCode == HttpCode.BadRequest) && !ssl) {
                        request.Attempts = 0;
                        Console.WriteLine("Request returns " + rr.ResponseCode + " (" + rr.ResponseStatus.HttpCode.NameIdentifier + ") without using SSL. Trying again with SSL.");

                        return DispatchRequest(request, false, true);
                    } else if (rr.ResponseStatus.HttpCode == HttpCode.TooManyRequests) {
                        Console.WriteLine("Too many requests, waiting 30 seconds and trying again.");
                        Misc.Sleep(30000);
                        return DispatchRequest(request, false, ssl);

                    } else {
                        Console.WriteLine("Client error => " + rr.ResponseStatus.HttpCode.ToString());
                    }
                } else if (rr.ResponseStatus.HttpCode.IsServerError()) {
                    Console.WriteLine("Server error => " + rr.ResponseStatus.HttpCode.ToString());

                } else if (!rr.ResponseStatus.HttpCode.IsSuccess()) {
                    Console.WriteLine("Unsuccessful request => " + rr.ResponseStatus.HttpCode.ToString());
                }
            }
            if (request.RequestType == RequestType.Get && !request.HasProperty(RequestProperty.NoRef)
                && !request.HasProperty(RequestProperty.XMLHttpRequest) && app.GetType() == (typeof(WebApplication))) {
                WebApplication webApp = app as WebApplication;
                webApp.LastNavigatedUrl = request.Url;
            }
            RedirectionChain.Clear();

            if (request is ContentRequest) {
                ContentRequest contReq = (ContentRequest)request;

                if (contReq.HasBody() && contReq.ContentBody.Chunked) {
                    if (!contReq.ContentBody.ChunkHandler.IsFinished) {
                        if (contReq.HasProgressListener) {
                            contReq.ProgressListener.SetProgress(contReq.ContentBody.ChunkHandler.Progress);
                        }
                        request.Attempts = 0;
                        return DispatchRequest(request, false, ssl);
                    }
                }
            }
            if (!ConnectionSecurityHandler.Bypass(this as HttpSession<Application>, request, rr)) {
                return DispatchRequest(request);
            }
            if (IsPersistent) {
                //TODO save();
            }
            PreviousApp = app;
            request.Attempts = 0;
            return rr;
        }

        private RequestResponse FollowRedirects(Request request, RequestResponse rr, bool sideSession, bool ssl) {
            if (RedirectionChain.Count >= MaxRedirects) {
                RedirectionChain.Clear();
                request.Attempts = 0;

                return new RequestResponse(request.Url, request.ExpectedResponseCode, new ResponseStatus(HttpCode.Unknown, "Too many redirects")
                    , new StringResponseBody("Too many redirects through " + request.Url), null);
            }
            string redirectUrl = rr.GetLocation();

            if (string.IsNullOrEmpty(redirectUrl)) {
                Console.WriteLine("No redirect url found on redirect response (" + request.RequestType.NameIdentifier + " => " + request.Url + ")");
                return rr;
            }
            if (!redirectUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)) {
                redirectUrl = HttpUtil.GetBaseUrl(request.Url) + redirectUrl;
            }
            if (HttpConfig.DebugMode) {
                Console.WriteLine("Redirect (" + rr.ResponseCode + ") => " + redirectUrl + " (Location: " + rr.GetLocation() + ")");
            }
            RedirectionChain.Add(request);
            request.Attempts = 0;

            Console.WriteLine("Move Permanently(" + HttpCode.MovedPermanently.Code + "): " + (rr.ResponseStatus.HttpCode == HttpCode.MovedPermanently));
            Console.WriteLine("Redir url = request url => " + redirectUrl.Equals(request.Url, StringComparison.OrdinalIgnoreCase));

            if (rr.ResponseStatus.HttpCode == HttpCode.MovedPermanently || redirectUrl.Equals(request.Url, StringComparison.OrdinalIgnoreCase)) {
                if (ssl) {
                    return new RequestResponse(request.Url, request.ExpectedResponseCode, new ResponseStatus(HttpCode.Unknown, "Source url is same as redirect url")
                    , new StringResponseBody("Retrieved same redirect url as the url we're already on while using SSL (" + redirectUrl + ")"), null);
                }
                return DispatchRequest(request, sideSession, true);
            }
            Request redirReq = new GetRequest(redirectUrl, HttpCode.Ok, request.GetProps());
            redirReq.CustomHeaders.Kvps.AddRange(request.CustomHeaders.Kvps);
            redirReq.SavePath = request.SavePath;
            redirReq.ProgressListener = request.ProgressListener;
            redirReq.Port = request.Port;
            redirReq.CachePolicy = request.CachePolicy;

            return DispatchRequest(redirReq);
        }

        public FileLink Download(string savePath, Request request) {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            request.SavePath = savePath;
            RequestResponse rr = DispatchRequest(request);

            if (!rr.Validate()) {
                Console.WriteLine("Failed to download from " + request.Url + " (" + rr.ResponseCode + ")");
                return null;
            }
            FileLink fileLink = ((FileResponseBody)rr.ResponseBody).GetBody();

            if (fileLink is null) {
                Console.WriteLine("Failed to receive file after download from " + request.Url);
                return null;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            if (HttpConfig.DebugMode) {
                Console.WriteLine("Downloaded " + savePath + " in " + ts.TotalSeconds + " seconds");
            }
            return fileLink;
        }

        public string FetchIP() {
            try {
                RequestResponse rr = DispatchRequest(new GetRequest("https://api.ipify.org/", RequestProperty.NoRef));

                if (!rr.Validate()) {
                    return "Failed to fetch IP";
                }
                return rr.GetResponseContent();

            } catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
                return "Failed to fetch IP (Exception)";
            }
        }

        private TA CreateApp(string domain) {
            TA app = InstantiateApp(domain);
            Apps.Add(app);
            return app;
        }

        private TA GetAppByUrl(string url) {
            string domain = HttpUtil.GetDomain(url);

            foreach (TA app in Apps) {
                if (app.Domain.Equals(domain, StringComparison.Ordinal)) {
                    return app;
                }
            }
            return CreateApp(domain);
        }

        public HttpSession<TA> ClearData() {
            foreach (TA app in Apps) {
                app.Clear();
            }
            return this;
        }

        public HttpSession<TA> OpenSideSession() {
            HttpSession<TA> sideSession = InstantiateSideSession(SessionPolicy, BrowserCfg, Proxy);
            sideSession.AutoRedirectDisabled = AutoRedirectDisabled;
            return sideSession;
        }

        protected abstract HttpSession<TA> InstantiateSideSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy);

        public override bool Equals(object obj) {
            if (obj is null || !(GetType() != typeof(HttpSession<TA>))) {
                return false;
            }
            return Uuid.Equals(((HttpSession<TA>)obj).Uuid, StringComparison.Ordinal);
        }

        public override int GetHashCode() {
            return MathUtil.ParseInt(Uuid, true, base.GetHashCode());
        }

    }
}
