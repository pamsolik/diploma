using System.ComponentModel.DataAnnotations;
using Core.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cars.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginWith2FaModel : PageModel
{
    private readonly ILogger<LoginWith2FaModel> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginWith2FaModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginWith2FaModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty] public InputModel Input { get; set; } = new();

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(bool rememberMe, string? returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new InvalidOperationException(
                "Nie można załadować użytkownika uwierzytelniania dwuskładnikowego.");

        ReturnUrl = returnUrl;
        RememberMe = rememberMe;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        returnUrl ??= Url.Content("~/");

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
            throw new InvalidOperationException(
                "Nie można załadować użytkownika uwierzytelniania dwuskładnikowego.");

        var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result =
            await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe,
                Input.RememberMachine);

        if (result.Succeeded)
        {
            _logger.LogInformation("User with ID '{UserId}' logged in with 2fa", user.Id);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User with ID '{UserId}' account locked out", user.Id);
            return RedirectToPage("./Lockout");
        }

        _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'", user.Id);
        ModelState.AddModelError(string.Empty, "Nieprawidłowy kod uwierzytelniający.");
        return Page();
    }

    public class InputModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0} musi składać się z od {2} do {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Kod uwierzytelniający")]
        public string TwoFactorCode { get; set; } = string.Empty;

        [Display(Name = "Zapamiętaj to urządzenie")]
        public bool RememberMachine { get; set; }
    }
}