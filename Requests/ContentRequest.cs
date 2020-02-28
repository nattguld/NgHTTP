using NgHTTP.Content;
using NgHTTP.Content.Bodies;
using NgHTTP.Util;

namespace NgHTTP.Requests {
    public abstract class ContentRequest : Request {

        public static readonly EmptyBody EmptyBody = new EmptyBody();

        public ContentBody ContentBody {get; set;}


        public ContentRequest(RequestType requestType, string url, HttpCode expectedResponseCode, params RequestProperty[] props)
            : base(requestType, url, expectedResponseCode, props) {
            ContentBody = EmptyBody;
        }

        public override bool HasBody() {
            return ContentBody != null;
        }

        public ContentRequest SetBody(ContentBody body) {
            ContentBody = body;
            return this;
        }

    }
}
