using NgUtil;
using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Proxies.Authentication {
    public sealed class ProxyCredentials : StringStringKeyValuePair {

        private string base64Auth;


        public ProxyCredentials(string username, string password) : base(username, password) { }

        public string GetBase64Auth() {
            if (string.IsNullOrEmpty(base64Auth)) {
                base64Auth = Converter.ToBase64(Key + ":" + Value);
            }
            return base64Auth;
        }

        public override string ToString() {
            return Key + ":" + Value;
        }

    }
}
