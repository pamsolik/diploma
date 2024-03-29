﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using Core.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty] public InputModel Input { get; set; } = new();

    public IActionResult OnGet(string? code = null)
    {
        if (code == null) return BadRequest("A code must be supplied for password reset.");

        Input = new InputModel
        {
            Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(Input?.Email);
        if (user == null)
            // Don't reveal that the user does not exist
            return RedirectToPage("./ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, Input?.Code, Input?.Password);
        if (result.Succeeded) return RedirectToPage("./ResetPasswordConfirmation");

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi składać się z od {2} do {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage =
                "Hasło musi mieć conajmniej 8 znaków i posiadać conajmniej: 1 małą literę, 1 wielką literę, 1 cyfrę oraz znak specjalny")]
        [DataType(DataType.Password, ErrorMessage = "Hasło nie spełnia wymagań")]
        [Display(Name = "Nowe hasło")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
}