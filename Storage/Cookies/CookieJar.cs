using NgUtil.Debugging.Contracts;
using System;
using System.Collections.Generic;

namespace NgHTTP.Storage.Cookies {
    public sealed class CookieJar {

        public List<Cookie> Cookies { get; } = new List<Cookie>();


        public CookieJar Add(Cookie cookie) {
            EmptyParamContract.Validate(cookie);

            Cookies.Add(cookie);
            return this;
        }

        public CookieJar ReplaceOrAdd(Cookie cookie) {
            EmptyParamContract.Validate(cookie);

            return Remove(GetByName(cookie.Name))
                .Add(cookie);
        }

        public CookieJar ImportCookies(List<Cookie> cookies) {
            EmptyParamContract.Validate(cookies);

            Cookies.AddRange(cookies);
            return this;
        }

        public CookieJar Remove(Cookie cookie) {
            if (cookie is null) {
                return this;
            }
            Cookies.Remove(cookie);
            return this;
        }

        public Cookie GetByName(string name, bool ignoreCase = true) {
            foreach (Cookie cookie in Cookies) {
                if (cookie.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) {
                    return cookie;
                }
            }
            return null;
        }

        public CookieJar Empty() {
            Cookies.Clear();
            return this;
        }

    }
}
