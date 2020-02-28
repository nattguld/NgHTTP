

namespace NgHTTP.Storage.Applications.Impl {
    public sealed class MobileApplication : Application {

        public string Referer { get; set; }

        public bool HasReferer => !string.IsNullOrEmpty(Referer);


        public MobileApplication(string domain) : base(domain) { }

        public override Application Clear() {
            Referer = null;
            return base.Clear();
        }

        public override string ToString() {
            return "MobileApplication: " + Domain;
        }

    }
}
