
namespace Models
{
    public class Response<T>
    {
        public string? ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public T? Result { get; set; }
    }
}
