using NgHTTP.Requests.Responses;
using NgHTTP.Sessions;
using NgHTTP.Storage;

namespace NgHTTP.Requests.Security {
    public interface IConnectionSecurity {


        public bool Encountered(HttpSession<Application> session, Request req, RequestResponse rr);

        public bool Bypass(HttpSession<Application> session, Request req, RequestResponse rr);

        public string GetName();

    }
}
