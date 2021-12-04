using System.Threading.Tasks;
using Cars.Managers.Implementations;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cars.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly IAppUserManager _appUserManager;

        public PersonalDataModel(ILogger<PersonalDataModel> logger, IAppUserManager appUserManager)
        {
            _logger = logger;
            _appUserManager = appUserManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var userId = _appUserManager.GetUserId(User);
            var user = await _appUserManager.FindUser(userId);
            if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");
            _logger.LogInformation("User information downloaded");
            return Page();
        }
    }
}