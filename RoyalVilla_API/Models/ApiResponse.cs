namespace RoyalVilla_API.Models
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        
        public string Message { get; set; }

        public TData? Data { get; set; }

        public object? Errors   { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;


         
    }
}
