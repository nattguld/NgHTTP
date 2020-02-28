using NgHTTP.Storage.Cookies;

namespace NgHTTP.Storage.Applications.Impl {
    public sealed class WebApplication : Application {

        public CookieJar CookieJar { get; }

        public string LastNavigatedUrl { get; set; }

        public bool HasNavigated => !string.IsNullOrEmpty(LastNavigatedUrl);


        public WebApplication(string domain) : base(domain) {
            CookieJar = new CookieJar();
        }

        public override Application Clear() {
            CookieJar.Empty();
            LastNavigatedUrl = null;
            return base.Clear();
        }

        public override string ToString() {
            return "WebApplication: " + Domain;
        }

    }
}
