using NgHTTP.Requests.Responses.Bodies;
using NgHTTP.Requests.Responses.Interpretors.Impl;
using System;
using System.IO;

namespace NgHTTP.Requests.Responses.Interpretors {
    public abstract class ResponseInterpretor<T> : BaseResponseInterpretor {


        public ResponseInterpretor(long bodySize) : base(bodySize) { }

        public abstract T Interpret(Stream stream);

    }
}
