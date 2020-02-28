using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class ConnectRequest : Request {


        public ConnectRequest(string url, params RequestProperty[] props)
            : base(RequestType.Connect, url, HttpCode.Ok, props) {
        }

        public ConnectRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props)
            : base(RequestType.Connect, url, expectedResponseCode, props) {
        }

        public override bool HasBody() {
            return false;
        }
    }

}
