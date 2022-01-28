using System.Net;
using System.Text.Json;

namespace Core.Exceptions;

public class AppBaseException : Exception
{
    public AppBaseException(HttpStatusCode status, string msg) : base(msg)
    {
        Status = status;
    }

    public HttpStatusCode Status { get; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}