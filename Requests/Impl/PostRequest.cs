using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class PostRequest : ContentRequest {


        public PostRequest(string url, params RequestProperty[] props) 
            : base(RequestType.Post, url, HttpCode.Ok, props) { }

        public PostRequest(string url, HttpCode expectedResponseCode, params RequestProperty[] props) 
            : base(RequestType.Post, url, expectedResponseCode, props) { }

    }

}
