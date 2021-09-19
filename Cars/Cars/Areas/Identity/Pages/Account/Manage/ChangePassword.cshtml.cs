using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cars.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword) return RedirectToPage("./SetPassword");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //throw new AppBaseException(HttpStatusCode.Unauthorized, "TEST");
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully");
            StatusMessage = "Twoje hasło zostało zmienione.";

            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Aktualne hasło")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} musi musi składać sie z od {2} do {1} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nowe hasło")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź nowe hasło")]
            [Compare("NewPassword", ErrorMessage = "Hasła nie są takie same.")]
            public string ConfirmPassword { get; set; }
        }
    }
}