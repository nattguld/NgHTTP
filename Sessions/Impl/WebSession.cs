using NgHTTP.Browser;
using NgHTTP.Headers;
using NgHTTP.Proxies;
using NgHTTP.Requests;
using NgHTTP.Requests.Impl;
using NgHTTP.Storage.Applications.Impl;
using NgHTTP.Util;
using NgUtil.Generics.Kvps.Impl;
using System;
using System.Text;

namespace NgHTTP.Sessions.Impl {
    public sealed class WebSession : HttpSession<WebApplication> {


        public WebSession() : base(HttpSessionPolicy.Default, new BrowserConfig(false), null) { }

        public WebSession(HttpSessionPolicy sessionPolicy) : base(sessionPolicy, new BrowserConfig(false), null) { }

        public WebSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg) : base(sessionPolicy, browserCfg, null) { }

        public WebSession(HttpSessionPolicy sessionPolicy, Proxy proxy) : base(sessionPolicy, new BrowserConfig(false), proxy) { }

        public WebSession(Proxy proxy, BrowserConfig browserCfg) : base(HttpSessionPolicy.Default, browserCfg, proxy) { }

        public WebSession(Proxy proxy) : base(HttpSessionPolicy.Default, new BrowserConfig(false), proxy) { }

        public WebSession(BrowserConfig browserCfg) : base(HttpSessionPolicy.Default, browserCfg, null) { }

        public WebSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy) : base(sessionPolicy, browserCfg, proxy) { }


        protected override WebApplication InstantiateApp(string domain) {
            return new WebApplication(domain);
        }

        protected override HttpSession<WebApplication> InstantiateSideSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy) {
            return new WebSession(sessionPolicy, browserCfg, proxy);
        }

        protected override void BuildRequestHeaders(StringStringKeyValuePairContainer kvpsContainer, Request req, WebApplication app, bool ssl, bool redirect) {
			bool xmlHttpRequest = req.HasProperty(RequestProperty.XMLHttpRequest);

			kvpsContainer.Put(HeaderKeys.Host, app.Domain);
			kvpsContainer.Put(HeaderKeys.Connection, HeaderValues.ConnectionKeepAlive);

			if (ssl) {
				kvpsContainer.Put(HeaderKeys.Dpr, "1");
			}
			kvpsContainer.Put(HeaderKeys.UserAgent, BrowserCfg.UserAgent);
			kvpsContainer.Put(HeaderKeys.Accept, req.ResponseEncType.AcceptValue);

			if ((req.RequestType == RequestType.Post && !xmlHttpRequest) || redirect || !string.IsNullOrEmpty(req.CachePolicy)) {
				kvpsContainer.Put(HeaderKeys.CacheControl, !string.IsNullOrEmpty(req.CachePolicy) ? req.CachePolicy : HeaderValues.MaxZeroCache);
			}
			if (!xmlHttpRequest) {
				kvpsContainer.Put(HeaderKeys.UpgradeInsecureRequests, "1");
			}
			if (xmlHttpRequest) {
				kvpsContainer.Put(HeaderKeys.XRequestedWith, HeaderValues.XmlHttpRequest);
			}
			if (BrowserCfg.DoNotTrack) {
				kvpsContainer.Put(HeaderKeys.Dnt, "1");
			}
			if (app.HasNavigated) {
				kvpsContainer.Put(HeaderKeys.Referer, app.LastNavigatedUrl);
			}
			kvpsContainer.Put(HeaderKeys.AcceptEncoding, HeaderValues.AcceptEncoding);
			kvpsContainer.Put(HeaderKeys.AcceptLanguage, BrowserCfg.Language);

			if (ssl) {
				BuildSecFetchHeaders(kvpsContainer, req, app);
			}
			if (req.RequestType == RequestType.Options) { //Sets the request method for options request
				kvpsContainer.Put(HeaderKeys.AccessControlRequestMethod, ((OptionsRequest)req).RequestTypeOption.Notation); //Sets the option request header
			}
			if (req.RequestType == RequestType.Options || xmlHttpRequest || req is ContentRequest) {
				kvpsContainer.Put(HeaderKeys.Origin, HttpUtil.GetBaseUrl(req.Url));
			}
			if (req.RequestType != RequestType.Options && app.CookieJar != null
					&& app.CookieJar.Cookies.Count > 0 && SessionPolicy != HttpSessionPolicy.NoData) {
				StringBuilder cookieSb = new StringBuilder();

				for (int i = 0; i < app.CookieJar.Cookies.Count; i++) {
					cookieSb.Append(app.CookieJar.Cookies[i].Name + "=" + app.CookieJar.Cookies[i].Value);

					if (i < (app.CookieJar.Cookies.Count - 1)) {
						cookieSb.Append("; ");
					}
				}
				kvpsContainer.Put(HeaderKeys.Cookie, cookieSb.ToString());
			}
			if (req.HasBody()) {
				((ContentRequest)req).ContentBody.Prepare(kvpsContainer);
			}
		}

		private void BuildSecFetchHeaders(StringStringKeyValuePairContainer reqHeaders, Request request, WebApplication app) {
			string lastNavigatedUrlDomain = app.LastNavigatedUrl is null ? null : HttpUtil.GetDomain(app.LastNavigatedUrl);

			reqHeaders.Put(HeaderKeys.SecFetchMode, request.HasProperty(RequestProperty.XMLHttpRequest)
					? HeaderValues.SecFetchCors : HeaderValues.SecFetchNavigate);  
			//request.getSecFetchMode()
			//TODO  nested-navigate  redirect to internal page by clicking button etc, no link? Form <<<
			//TODO same-origin/same-site/cross-site when called inside the page, like js scripts being called
			//boolean firstHostContact = !accessedHosts.contains(host) || (request.getRequestType() == RequestType.GET && ((GetRequest)request).isNoRef());
			bool sameOrigin = IsSameOrigin(app.Domain, lastNavigatedUrlDomain);
			bool sameSite = !sameOrigin && IsSameSiteDifferentHost(app.Domain, lastNavigatedUrlDomain);
			bool crossSite = !sameOrigin && !sameSite;

			if (AccessedHosts.Count > 0 && !crossSite && RedirectionChain.Count > 1) {
				for (int i = 0; i < RedirectionChain.Count; i++) {
					Request prevReq = RedirectionChain[i];
					string prevHost = HttpUtil.GetDomain(prevReq.Url);

					if (IsSameOrigin(app.Domain, prevHost)) {
						continue;
					}
					if (IsSameSiteDifferentHost(app.Domain, prevHost)) {
						sameSite = true;
						continue;
					}
					crossSite = true;
					break;
				}
			}
			if (AccessedHosts.Count == 0) {
				reqHeaders.Put(HeaderKeys.SecFetchSite, HeaderValues.SecFetchNone);
			} else if (crossSite) {
				reqHeaders.Put(HeaderKeys.SecFetchSite, HeaderValues.SecFetchCrossSite);
			} else if (sameSite) {
				reqHeaders.Put(HeaderKeys.SecFetchSite, HeaderValues.SecFetchSameSite);
			} else {
				reqHeaders.Put(HeaderKeys.SecFetchSite, HeaderValues.SecFetchSameOrigin);
			}
			if (!request.HasProperty(RequestProperty.XMLHttpRequest)) {
				reqHeaders.Put(HeaderKeys.SecFetchUser, HeaderValues.SecFetchUser);
			}
		}

		private static bool IsSameOrigin(string host, string otherHost) {
			return otherHost is null || host.Equals(otherHost, StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsSameSiteDifferentHost(string host, string otherHost) {
			if (otherHost is null || host.Equals(otherHost, StringComparison.OrdinalIgnoreCase)) {
				return false;
			}
			return host.Contains(otherHost, StringComparison.OrdinalIgnoreCase) 
				|| otherHost.Contains(host, StringComparison.OrdinalIgnoreCase);
		}

	}
}
