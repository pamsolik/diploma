namespace Cars.Models.View
{
    public class ApiAnswer
    {
        public ApiAnswer(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}