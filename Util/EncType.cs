using NgUtil.Generics.Enums;

namespace NgHTTP.Util {
    public sealed class EncType : ExtendedEnum<EncType> {

        public string ContentValue { get; }

        public string AcceptValue { get; }


        private EncType(string name, string contentValue, string acceptValue) : base(name) {
            ContentValue = contentValue;
            AcceptValue = acceptValue;
        }

        public static readonly EncType PlainJson = new EncType("Plain Json", "application/json", "application/json");
        public static readonly EncType Json = new EncType("Json", "application/json", "application/json, text/javascript, */*; q=0.01");
        public static readonly EncType Xml = new EncType("XML", "text/xml;charset=utf-8", "text/xml, application/xml, text/javascript, */*; q=0.01");
        public static readonly EncType UrlEncoded = new EncType("URL encoded", "application/x-www-form-urlencoded; charset=UTF-8"
            , "text/html,application/xhtml+xml,application/xml;q=0.9, image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
        public static readonly EncType Multipart = new EncType("Multipart", "multipart/form-data", "text/html,application/xhtml+xml,application/xml;q=0.9, image/webp,image/apng,*/*;q=0.8");
        public static readonly EncType Stream = new EncType("Octet-Stream", "application/octet-stream", "*/*");
        public static readonly EncType All = new EncType("All", "*/*", "*/*");
        public static readonly EncType TextHtml = new EncType("text/html", "text/html, */*; q=0.01", "text/html, */*; q=0.01");
        public static readonly EncType PlainText = new EncType("text/plain", "text/plain", "text/plain");

    }
}
