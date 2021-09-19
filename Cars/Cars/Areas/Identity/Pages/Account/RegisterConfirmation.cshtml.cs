using System.Text;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Cars.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly IEmailSender _sender;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null) return RedirectToPage("/Index");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound($"Nie znaleziono użytkownika z adresem '{email}'.");

            Email = email;
            return Page();
        }
    }
}