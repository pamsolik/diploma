using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Services.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cars.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/files/upload")]
    public class FilesController : ControllerBase
    {
        //private readonly IAdminService _adminService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
           
        }
        
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files[0];
                var folderName = Path.Combine("Resources", "Images", "Temp");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                
                if (file.Length <= 0) return BadRequest();
                var name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                if (name == null) return BadRequest();
                var fileName = $"File_{User.FindFirstValue(ClaimTypes.NameIdentifier)}_{DateTime.Now:yyyy-dd-MM-HH-mm-ss}{Path.GetExtension(name).Trim('"')}";
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);
                
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
                return Ok(new { dbPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}