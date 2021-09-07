using System;
using System.Net;

namespace Cars.Models.Exceptions
{
    public class AppBaseException : Exception
    {
        public AppBaseException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }

        public HttpStatusCode Status { get; }
    }
}