using Cars.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        [Route("api/error")]
        public ErrorDetails Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // Your exception
            const int code = 500; // Internal Server Error by default

            //if      (exception is MyNotFoundException) code = 404; // Not Found
            //else if (exception is MyUnauthException)   code = 401; // Unauthorized
            //else if (exception is MyException)         code = 400; // Bad Request

            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            _logger.LogError($"Exception thrown: {exception.Message}");

            return new ErrorDetails(exception); // Your error model
        }
    }
}