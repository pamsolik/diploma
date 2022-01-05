using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Cars.Controllers;

[Authorize(Roles = "User,Recruiter,Admin")]
[ApiController]
[Route("api/files/upload")]
public class FilesController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;
    private readonly ILogger<FilesController> _logger;

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

        if (name is null)
        {
            badRequest = BadRequest("File name cannot be empty");
            return true;
        }

        badRequest = null;
        return false;
    }
}