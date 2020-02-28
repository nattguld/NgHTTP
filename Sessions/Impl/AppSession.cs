using NgHTTP.Browser;
using NgHTTP.Headers;
using NgHTTP.Proxies;
using NgHTTP.Requests;
using NgHTTP.Requests.Impl;
using NgHTTP.Storage.Applications.Impl;
using NgHTTP.Util;
using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Sessions.Impl
{
    public sealed class AppSession : HttpSession<MobileApplication> {


        public AppSession() : base(HttpSessionPolicy.Default, new BrowserConfig(true), null) { }

        public AppSession(HttpSessionPolicy sessionPolicy) : base(sessionPolicy, new BrowserConfig(true), null) { }

        public AppSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg) : base(sessionPolicy, browserCfg, null) { }

        public AppSession(HttpSessionPolicy sessionPolicy, Proxy proxy) : base(sessionPolicy, new BrowserConfig(true), proxy) { }

        public AppSession(Proxy proxy, BrowserConfig browserCfg) : base(HttpSessionPolicy.Default, browserCfg, proxy) { }

        public AppSession(Proxy proxy) : base(HttpSessionPolicy.Default, new BrowserConfig(true), proxy) { }

        public AppSession(BrowserConfig browserCfg) : base(HttpSessionPolicy.Default, browserCfg, null) { }

        public AppSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy) : base(sessionPolicy, browserCfg, proxy) { }


		protected override MobileApplication InstantiateApp(string domain) {
			return new MobileApplication(domain);
		}

		protected override HttpSession<MobileApplication> InstantiateSideSession(HttpSessionPolicy sessionPolicy, BrowserConfig browserCfg, Proxy proxy) {
			return new AppSession(sessionPolicy, browserCfg, proxy);
		}

		protected override void BuildRequestHeaders(StringStringKeyValuePairContainer kvpsContainer, Request req, MobileApplication app, bool ssl, bool redirect) {
			kvpsContainer.Put(HeaderKeys.Host, app.Domain);
			kvpsContainer.Put(HeaderKeys.Connection, HeaderValues.ConnectionKeepAlive);
			kvpsContainer.Put(HeaderKeys.Accept, req.ResponseEncType.AcceptValue);
			kvpsContainer.Put(HeaderKeys.UserAgent, BrowserCfg.UserAgent);

			if (app.HasReferer) {
				kvpsContainer.Put(HeaderKeys.Referer, app.Referer);
			}
			kvpsContainer.Put(HeaderKeys.AcceptEncoding, HeaderValues.AcceptEncoding);
			kvpsContainer.Put(HeaderKeys.AcceptLanguage, BrowserCfg.Language);

			if (req.RequestType == RequestType.Options) { //Sets the request method for options request
				kvpsContainer.Put(HeaderKeys.AccessControlRequestMethod, ((OptionsRequest)req).RequestTypeOption.Notation); //Sets the option request header
			}
			if (req.RequestType == RequestType.Options || req is ContentRequest) {
				kvpsContainer.Put(HeaderKeys.Origin, HttpUtil.GetBaseUrl(req.Url));
			}
			if (req.RequestType == RequestType.Options || req is ContentRequest) {
				kvpsContainer.Put(HeaderKeys.Origin, HttpUtil.GetBaseUrl(req.Url));
			}
			if (req.HasBody()) {
				((ContentRequest)req).ContentBody.Prepare(kvpsContainer);
			}
		}

    }
}
