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

        public Response(string message, string? errorCode = null)
        {
            Message = message;
            Succeded = false;
            ErrorCode = errorCode;
        }


        public bool Succeded { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorCode { get; set; }

        public string Message { get; set; }


        public T Result { get; set; }

    }
}
