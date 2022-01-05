using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Core.DataModels;
using Core.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<RegisterModel> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    [BindProperty] public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (!ModelState.IsValid) return Page();
        var user = new ApplicationUser
        {
            UserName = Input?.Email,
            Email = Input?.Email,
            Name = Input?.Name,
            Surname = Input?.Surname,
            ProfilePicture = ImgPath.BaseProfilePic
        };

        var result = await _userManager.CreateAsync(user, Input?.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");

            if (Input is not null && Input.IsRecruiter)
                await _userManager.AddToRoleAsync(user, "Recruiter");

            _logger.LogInformation("User created a new account with password");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { area = "Identity", userId = user.Id, code, returnUrl },
                Request.Scheme);

            if (callbackUrl != null)
                await _emailSender.SendEmailAsync(Input?.Email, "Potwierdź swój email",
                    $"Potwierdź swoje konto <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                return RedirectToPage("RegisterConfirmation", new { email = Input?.Email, returnUrl });

            await _signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        // If we got this far, something failed, redisplay form
        return Page();
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Imię nie może być dłuższe niż 200 znaków.")]
        [Display(Name = "Imię")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Nazwisko nie może być dłuższe niż 200 znaków.")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi składać się z od {2} do {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Jestem rekruterem")] public bool IsRecruiter { get; set; }
    }
}