using NgUtil.Generics.Enums;
using System;

namespace NgHTTP.Util {
    public sealed class HttpVersion : ExtendedEnum<HttpVersion> {

        public string Notation { get; }


        private HttpVersion(string name, string notation) : base(name) {
            Notation = notation;
        }

        public static readonly HttpVersion Http_1_0 = new HttpVersion("HTTP 1.0", "HTTP/1.0");
        public static readonly HttpVersion Http_1_1 = new HttpVersion("HTTP 1.1", "HTTP/1.1");
        public static readonly HttpVersion Http_2_0 = new HttpVersion("HTTP 2.0", "HTTP/2.0");

        public static HttpVersion Parse(string httpVersion) {
            foreach (HttpVersion hv in Values()) {
                if (hv.Notation.Equals(httpVersion, StringComparison.OrdinalIgnoreCase)) {
                    return hv;
                }
            }
            return Http_1_1;
        }

    }

}
