namespace NgHTTP.Requests.Responses.Bodies {
    public interface IResponseBody<T> : IBaseResponseBody
    {


        public T GetBody();

    }

}
