using NgUtil.Generics.Enums;

namespace NgHTTP.Requests {
    public sealed class RequestType : ExtendedEnum<RequestType> {

        public string Notation { get; }

        public bool Body { get; }


        private RequestType(string name, string notation, bool body) : base(name) {
            Notation = notation;
            Body = body;
        }

        public static readonly RequestType Post = new RequestType("Post", "POST", true);
        public static readonly RequestType Put = new RequestType("Put", "PUT", false);
        public static readonly RequestType Get = new RequestType("Get", "GET", true);
        public static readonly RequestType Options = new RequestType("Options", "OPTIONS", true);
        public static readonly RequestType Delete = new RequestType("Delete", "DELETE", true);
        public static readonly RequestType Head = new RequestType("Head", "HEAD", false);
        public static readonly RequestType Trace = new RequestType("Trace", "TRACE", false);
        public static readonly RequestType Connect = new RequestType("Connect", "CONNECT", false);
        public static readonly RequestType Patch = new RequestType("Patch", "PATCH", true);

    }
}
