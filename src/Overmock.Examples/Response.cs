using System.Text.Json.Serialization;

namespace Overmock.Examples
{
    public class Response
    {
        public Response()
        {
        }

        protected Response(string errorDetails) => ErrorDetails = errorDetails;

        public bool Success => ErrorDetails == null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorDetails { get; set; }

        public static implicit operator Response(Exception obj) => new(obj.Message);
    }
 
    public class Response<T> : Response
    {
        protected Response(string errorDetails) : base(errorDetails)
        {
        }

        protected Response(T? obj) => Result = obj;

        protected Response(List<T> results) => Results = results;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Result { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<T>? Results { get; set; }

        public static implicit operator Response<T>(T? obj) => new(obj);

        public static implicit operator Response<T>(List<T> results) => new(results);

        public static implicit operator Response<T>(Exception obj) => new(obj.Message);
    }
}
