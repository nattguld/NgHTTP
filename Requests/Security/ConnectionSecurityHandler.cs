using NgHTTP.Requests.Responses;
using NgHTTP.Sessions;
using NgHTTP.Storage;
using NgHTTP.Util;
using System;
using System.Collections.Generic;

namespace NgHTTP.Requests.Security {
    public static class ConnectionSecurityHandler {

        private static Dictionary<IConnectionSecurity, string> connSecImpls = new Dictionary<IConnectionSecurity, string>();


        public static void Register(IConnectionSecurity connSec, string domain) {
            connSecImpls.Add(connSec, domain);
        }

        private static List<IConnectionSecurity> GetImplementations(string domain) {
            List<IConnectionSecurity> implementations = new List<IConnectionSecurity>();

            foreach (KeyValuePair<IConnectionSecurity, string> entry in connSecImpls) {
                if (entry.Value.Equals(domain, System.StringComparison.OrdinalIgnoreCase)) {
                    implementations.Add(entry.Key);
                }
            }
            return implementations;
        }

        public static bool Bypass(HttpSession<Application> session, Request req, RequestResponse rr) {
            string domain = HttpUtil.GetDomain(req.Url);
            List<IConnectionSecurity> implementations = GetImplementations(domain);

            if (implementations.Count == 0) {
                return true;
            }
            foreach (IConnectionSecurity connSec in implementations) {
                if (!connSec.Encountered(session, req, rr)) {
                    continue;
                }
                Console.WriteLine("Connection security (" + connSec.GetName() + ") encountered => " + req.Url);

                if (!connSec.Bypass(session, req, rr)) {
                    Console.WriteLine("Connection security (" + connSec.GetName() + ") failed to bypass => " + req.Url);
                    return false;
                }
                if (!connSec.Encountered(session, req, rr)) {
                    Console.WriteLine("Connection security (" + connSec.GetName() + ") encountered again after solving => " + req.Url);
                    return false;
                }
                Console.WriteLine("Connection security (" + connSec.GetName() + ") bypassed => " + req.Url);
            }
            return true;
        }

    }
}
