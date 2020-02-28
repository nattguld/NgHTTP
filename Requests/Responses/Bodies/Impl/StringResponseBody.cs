using System;

namespace NgHTTP.Requests.Responses.Bodies.Impl {
    public class StringResponseBody : IResponseBody<string> {

        private readonly string content;


        public StringResponseBody(string content) {
            this.content = content;
        }

        public string GetBody() {
            return content;
        }

        /*public String GetBody<String>() {
            return (String)Convert.ChangeType(content, typeof(String));
        }*/

    }
}
