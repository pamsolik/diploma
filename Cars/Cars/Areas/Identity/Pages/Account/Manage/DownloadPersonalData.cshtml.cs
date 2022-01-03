using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cars.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel : PageModel
{
    private readonly IAppUserManager _appUserManager;
    private readonly ILogger<DownloadPersonalDataModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public DownloadPersonalDataModel(IAppUserManager appUserManager, ILogger<DownloadPersonalDataModel> logger,
        UserManager<ApplicationUser> userManager)
    {
        _appUserManager = appUserManager;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = _appUserManager.GetUserId(User);
        var user = await _appUserManager.FindUser(userId);
        if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

        _logger.LogInformation("User with ID '{UserId}' asked for their personal data", userId);

        // Only include personal data for download
        var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        var personalData = personalDataProps
            .ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");

        var logins = await _userManager.GetLoginsAsync(user);
        foreach (var l in logins) personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);

        personalData.Add("Education", JsonSerializer.Serialize(user.Education));
        personalData.Add("Experience", JsonSerializer.Serialize(user.Experience));
        personalData.Add("Skills", JsonSerializer.Serialize(user.Skills));

        Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
        return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
    }
}