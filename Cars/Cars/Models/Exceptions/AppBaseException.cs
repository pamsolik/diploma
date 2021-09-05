using System;
using System.Net;

namespace Cars.Models.Exceptions
{
    public class AppBaseException : Exception
    {
        public HttpStatusCode Status { get; private set; }

        public AppBaseException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }
    }
}