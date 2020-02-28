using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class DeleteRequest : ContentRequest {


        public DeleteRequest(string url, params RequestProperty[] props) 
            : base(RequestType.Delete, url, HttpCode.Ok, props) { }

        public DeleteRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props) 
            : base(RequestType.Delete, url, expectedResponseCode, props) { }

    }

}
