using System;
using System.Collections.Generic;
using System.Text;

namespace NgHTTP.Storage.Cookies {
    public sealed class Cookie {

        public string Name { get; }

        public string Value { get; set; }

        public string Expires { get; set; }

        public string Path { get; set; }

        public string Domain { get; set; }

        public bool Secure { get; set; }

        public long MaxAge { get; set; }

        public string SameSite { get; set; }


        public Cookie(string name, string value, string domain) {
            Name = name;
            Value = value;
            Domain = domain;
        }

        public override string ToString() {
            return Name + "=" + Value;
        }

    }
}
