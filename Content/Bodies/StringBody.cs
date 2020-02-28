using NgHTTP.Streams;
using NgHTTP.Util;
using System;
using System.Text;

namespace NgHTTP.Content.Bodies {
    public sealed class StringBody : ContentBody {

        private readonly string content;


        public StringBody(EncType encType, string content) : base(encType) {
            this.content = content;
        }

        //Parent =>
        //public abstract T GetContent<T>();

        public override String GetContent<String>() {
            return (String)Convert.ChangeType(content, typeof(String));
            //return content;
        }

        protected override void Build(IHttpStreamable httpStream, bool prepare) {
            httpStream.AppendString(Encoding.UTF8.GetString(Encoding.Default.GetBytes(content)));
        }

    }
}
