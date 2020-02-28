using NgHTTP.Util;

namespace NgHTTP.Requests.Impl {
    public sealed class OptionsRequest : Request {

        public RequestType RequestTypeOption { get;}


        public OptionsRequest(RequestType requestTypeOption, string url, params RequestProperty[] props)
            : this(requestTypeOption, url, HttpCode.Ok, props) {
        }

        public OptionsRequest(RequestType requestTypeOption, string url, HttpCode expectedResponseCode, params RequestProperty[] props)
            : base(RequestType.Options, url, expectedResponseCode, props) {
            RequestTypeOption = requestTypeOption;
        }

        public override bool HasBody() {
            return false;
        }
    }

}
