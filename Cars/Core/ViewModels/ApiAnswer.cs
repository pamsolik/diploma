namespace Core.ViewModels;

public class ApiAnswer
{
    public ApiAnswer(string message)
    {
        Message = message;
    }

    public ApiAnswer(string message, int id)
    {
        Message = message;
        Id = id;
    }

    public string Message { get; set; }
    public int Id { get; set; }
}