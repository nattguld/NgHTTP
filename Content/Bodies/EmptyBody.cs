using NgHTTP.Headers;
using NgHTTP.Streams;
using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Content.Bodies {
    public sealed class EmptyBody : ContentBody {


        public EmptyBody() : base(null) { }

        public override Object GetContent<Object>() {
            return default;
        }

        protected override void Build(IHttpStreamable httpStream, bool prepare) {
            //Empty
        }

        protected override void SetContentHeaders(StringStringKeyValuePairContainer headers) {
            headers.Put(HeaderKeys.ContentLength, "0");
        }

    }
}
