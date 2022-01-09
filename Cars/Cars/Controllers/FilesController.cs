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
    public async Task<IActionResult?> Upload()
    {
        var formCollection = await Request.ReadFormAsync();
        {
            var file = formCollection.Files[0];
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Uploading file user: {User}", user);
            var res = await _fileUploadService.SaveFile(file, user);
            return Ok(res);
        }
    }
}