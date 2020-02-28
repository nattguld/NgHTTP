using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class PutRequest : ContentRequest {


        public PutRequest(string url, params RequestProperty[] props) 
            : base(RequestType.Put, url, HttpCode.Ok, props) { }

        public PutRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props) 
            : base(RequestType.Put, url, expectedResponseCode, props) { }

    }

}
