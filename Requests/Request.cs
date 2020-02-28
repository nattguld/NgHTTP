using NgHTTP.Requests.Responses.Listeners;
using NgHTTP.Util;
using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Requests {

    public abstract class Request {

        public RequestType RequestType { get; }

        public string Url { get; }

        public int Port { get; set; }

        public HttpCode ExpectedResponseCode { get; }

        public StringStringKeyValuePairContainer CustomHeaders {get;}

        private readonly RequestProperty[] Props;

        public EncType ResponseEncType { get; }

        public string CachePolicy { get; set; }

        public string SavePath { get; set; }

        public int Attempts { get; set; }

        public ProgressListener ProgressListener { get; set; }

        public bool HasProgressListener => ProgressListener != null;


        public Request(RequestType requestType, string url, HttpCode expectedResponseCode, params RequestProperty[] props) {
            RequestType = requestType;
            Url = url;
            ExpectedResponseCode = expectedResponseCode;
            CustomHeaders = new StringStringKeyValuePairContainer();
            ResponseEncType = EncType.UrlEncoded;
            Port = 80;
            Props = props;
        }

        public abstract bool HasBody();

        public bool HasProperty(RequestProperty requestProperty) {
            if (Props is null || Props.Length <= 0) {
                return false;
            }
            foreach (RequestProperty prop in Props) {
                if (prop == requestProperty) {
                    return true;
                }
            }
            return false;
        }

        public RequestProperty[] GetProps() {
            return (RequestProperty[])Props.Clone();
        }

    }
}
