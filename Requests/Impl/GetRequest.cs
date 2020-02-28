using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class GetRequest : Request {


        public GetRequest(string url, params RequestProperty[] props)
            : base(RequestType.Get, url, HttpCode.Ok, props) {
        }

        public GetRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props)
            : base(RequestType.Get, url, expectedResponseCode, props) {
        }

        public override bool HasBody() {
            return false;
        }
    }

}
