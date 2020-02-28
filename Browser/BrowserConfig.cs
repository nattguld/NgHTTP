using NgHTTP.Configs;
using NgHTTP.Util;

namespace NgHTTP.Browser {
    public class BrowserConfig {

        public HttpVersion HttpVersion { get; set; } = HttpVersion.Http_1_1;

        public bool IsMobile { get; set; }

        public string UserAgent { get; set; }

        public string Language { get; set; }

        public int ReadTimeout { get; set; }

        public int WriteTimeout { get; set; }

        public bool Caching { get; set; }

        public bool DoNotTrack { get; set; }

        public bool Debug { get; set; }


        public BrowserConfig(bool mobile) : this(mobile ? UserAgents.GetMobileUserAgent() : UserAgents.GetDesktopUserAgent(), mobile) {}

        public BrowserConfig(string userAgent, bool mobile = false) {
            UserAgent = userAgent;
            IsMobile = mobile;
            Language = "en-US,en;q=0.9";
            ReadTimeout = HttpConfig.ReadTimeout;
            WriteTimeout = HttpConfig.SendTimeout;
        }

    }
}
