﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Core.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.Areas.Identity.Pages.Account.Manage;

public class EmailModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmailModel(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public string Username { get; set; } = string.Empty;

    [Display(Name = "E-mail")] public string Email { get; set; } = string.Empty;

    public bool IsEmailConfirmed { get; set; }

    [TempData] public string StatusMessage { get; set; } = string.Empty;

    [BindProperty] public InputModel Input { get; set; } = new();

    private async Task LoadAsync(ApplicationUser user)
    {
        var email = await _userManager.GetEmailAsync(user);
        Email = email;

        Input = new InputModel
        {
            NewEmail = email
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var currentEmail = await _userManager.GetEmailAsync(user);

        if (Input.NewEmail != currentEmail)
        {
            var exists = await _userManager.FindByEmailAsync(Input.NewEmail);

            if (exists != null && exists.Id != user.Id)
            {
                StatusMessage = $"Użytkownik o adresie e-mail {Input.NewEmail} już istnieje.";
                return RedirectToPage();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                null,
                new { userId, email = Input.NewEmail, code },
                Request.Scheme);
            if (callbackUrl != null)
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Potwierdź swój e-mail",
                    $"Potwierdź swój e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");

            StatusMessage = "Wysłano link potwierdzający zmianę e-maila. Proszę sprawdzić e-mail.";
            return RedirectToPage();
        }

        StatusMessage = "Twój e-mail pozostaje bez zmian.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new { area = "Identity", userId, code },
            Request.Scheme);
        if (callbackUrl != null)
            await _emailSender.SendEmailAsync(
                email,
                "Potwierdź swój email",
                $"Potwierdź swój e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikając tutaj</a>.");

        StatusMessage = "E-mail weryfikacyjny został wysłany. Proszę sprawdzić e-mail.";
        return RedirectToPage();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Pole E-mail jest wymagane")]
        [EmailAddress(ErrorMessage = "E-mail nie jest poprawny")]
        [Display(Name = "Nowy adres e-mail")]
        public string NewEmail { get; set; } = string.Empty;
    }
}