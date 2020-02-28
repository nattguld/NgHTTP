using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class PatchRequest : ContentRequest {


        public PatchRequest(string url, params RequestProperty[] props) 
            : base(RequestType.Patch, url, HttpCode.Ok, props) { }

        public PatchRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props) 
            : base(RequestType.Patch, url, expectedResponseCode, props) { }

    }

}
