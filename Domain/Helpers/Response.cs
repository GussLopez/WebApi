using Microsoft.Identity.Client;

namespace WebApi103.Helpers

{
    public class Response<T>
    {
        public Response() { }

        public Response(T data, string message = null)
        {
            Succeded = true;
            Message = message;
            Result = data;
        }

        public Response(string message)
        {
            Message = message;
            Succeded = false;
        }


        public bool Succeded { get; set; }

        public string Message { get; set; }


        public T Result { get; set; }

    }
}
