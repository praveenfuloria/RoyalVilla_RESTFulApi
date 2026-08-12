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

        public static ApiResponse<TData> Create(bool suceess, int code, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = suceess,
                Code = code,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        public static ApiResponse<TData> Ok(string message, TData? data) => Create(true, 200, message,data);
        public static ApiResponse<TData> CreatedAt(string message, TData? data) => Create(true, 201, message, data);
        public static ApiResponse<TData> NoContent(string message="Operation Completed Successfully") => Create(true, 204, message);
        public static ApiResponse<TData> NotFound(string message="Resource Not Found") => Create(false, 404, message);
        public static ApiResponse<TData> BadRequest(string message, object? errors) => Create(false, 400, message, errors: errors);
        public static ApiResponse<TData> Conflict(string message) => Create(false, 409, message);

        public static ApiResponse<TData> Error(string message, object? errors) => Create(false, 500, message, errors:errors);




    }
}
