using System.ComponentModel.DataAnnotations;

namespace ecomove_back.Helpers
{
    public class Response<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
        public required bool IsSuccess { get; set; }
        public int CodeStatus { get; set; }
    }
}

