namespace apief
{
    public class ApiResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }

        public ApiResponse(bool success, string? message = null, object? data = null)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }
}