using System;

namespace Cars.Data
{
    public class ErrorDetails
    {
        public ErrorDetails(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }

        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}