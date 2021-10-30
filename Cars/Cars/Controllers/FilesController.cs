using System;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/files/upload")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IFileUploadService _fileUploadService;
        public FilesController(ILogger<FilesController> logger, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        //Limit: 25mb
        [HttpPost]
        [RequestSizeLimit(26214400L)]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files[0];
                
                if (ValidateFile(file, out var badRequest)) return badRequest;

                var res = await _fileUploadService.SaveFile(file, User.FindFirstValue(ClaimTypes.NameIdentifier));
               
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private bool ValidateFile(IFormFile file, out IActionResult badRequest)
        {
            var name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

            if (file.Length <= 0)
            {
                badRequest = BadRequest("File lenght is equal to 0");
                return true;
            }

            if (name == null)
            {
                badRequest = BadRequest("File name cannot be empty");
                return true;
            }

            badRequest = null;
            return false;
        }
    }
}