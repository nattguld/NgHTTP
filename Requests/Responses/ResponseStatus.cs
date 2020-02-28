using NgHTTP.Util;

namespace NgHTTP.Requests.Responses {
    public struct ResponseStatus {

        public HttpVersion HttpVersion { get; }

        public HttpCode HttpCode { get; }

        public string Message { get; }


        public ResponseStatus(HttpCode httpCode, string message) : this(HttpVersion.Http_1_1, httpCode, message) { }

        public ResponseStatus(HttpVersion httpVersion, HttpCode httpCode, string message) {
            HttpVersion = httpVersion;
            HttpCode = httpCode;
            Message = message;
        }

        public override string ToString() {
            return HttpVersion.NameIdentifier + " " + HttpCode.ToString() + " " + Message;
        }

    }

}
