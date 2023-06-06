
namespace KLG.Backend.Promotion.Models.Request
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public int Pages { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
}
