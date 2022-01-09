using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Managers.Interfaces;

namespace Cars.Areas.Identity.Pages.Account.Manage;

public class PersonalDataModel : PageModel
{
    private readonly IAppUserManager _appUserManager;
    private readonly ILogger<PersonalDataModel> _logger;

    public PersonalDataModel(ILogger<PersonalDataModel> logger, IAppUserManager appUserManager)
    {
        _logger = logger;
        _appUserManager = appUserManager;
    }

    public async Task<IActionResult> OnGet()
    {
        var userId = _appUserManager.GetUserId(User);
        await _appUserManager.FindUser(userId);
        _logger.LogInformation("User information downloaded");
        return Page();
    }
}