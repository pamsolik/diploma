﻿using System.ComponentModel.DataAnnotations;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cars.Areas.Identity.Pages.Account.Manage;

public class SetPasswordModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public SetPasswordModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty] public InputModel Input { get; set; } = new();

    [TempData] public string StatusMessage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var hasPassword = await _userManager.HasPasswordAsync(user);

        if (hasPassword) return RedirectToPage("./ChangePassword");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            foreach (var error in addPasswordResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Twoje hasło zostało ustawione.";

        return RedirectToPage();
    }

    public class InputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi składać się z od {2} do {1} znaków.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage =
                "Hasło musi mieć conajmniej 8 znaków i posiadać conajmniej: 1 małą literę, 1 wielką literę, 1 cyfrę oraz znak specjalny")]
        [DataType(DataType.Password, ErrorMessage = "Hasło nie spełnia wymagań")]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są takie same.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}