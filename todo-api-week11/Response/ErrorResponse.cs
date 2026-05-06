namespace todo_api_week11.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; }
        public DateTime Timestamp { get; set; }

        public ErrorResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public ErrorResponse(int statusCode, string message, Dictionary<string, string[]>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
            Timestamp = DateTime.UtcNow;
        }
    }
}
