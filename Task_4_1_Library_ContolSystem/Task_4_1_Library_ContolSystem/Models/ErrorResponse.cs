namespace Task_4_1_Library_ControlSystem.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? StackTrace { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(Exception exception)
        {
            Type = exception.GetType().Name;
            Message = exception.Message;
            Timestamp = DateTime.UtcNow;

#if DEBUG
            StackTrace = exception.StackTrace;
#endif
        }
    }
}