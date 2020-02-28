using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class HeadRequest : Request {


        public HeadRequest(string url, params RequestProperty[] props)
            : base(RequestType.Head, url, HttpCode.Ok, props) {
        }

        public HeadRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props)
            : base(RequestType.Head, url, expectedResponseCode, props) {
        }

        public override bool HasBody() {
            return false;
        }
    }

}
