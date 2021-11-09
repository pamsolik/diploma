using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Services.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static Cars.Services.Other.FileService;
using static Cars.Models.Enums.ImgPath;
using Duende.IdentityServer.Extensions;

namespace Cars.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IFileUploadService _fileUploadService;

        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IFileUploadService fileUploadService,
            ILogger<IndexModel> logger, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileUploadService = fileUploadService;
            _logger = logger;
            _userService = userService;
        }

        [Display(Name = "Nazwa użytkownika")] public string Username { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public string Filename { get; set; }


        public string ProfileUrl { get; set; }
        [BindProperty] public InputModel Input { get; set; }

        [BindProperty] public IFormFile Upload { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            if (user.ProfilePicture.IsNullOrEmpty()) user.ProfilePicture = BaseProfilePic;

            Filename = user.ProfilePicture;

            ProfileUrl = "https://" + HttpContext.Request.Host + "/" + Filename;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
                Surname = user.Surname,
                Description = user.Description,
                ProfilePicture = user.ProfilePicture,
                City = user.City,
                Github = user.Github,
                LinkedIn = user.LinkedIn,
                Skills = user.Skills,
                Education = user.Education,
                Experience = user.Experience
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                    StatusMessage = "Nieoczekiwany błąd podczas próby ustawienia numeru telefonu.";
            }

            CheckChanges(user);

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został zaktualizowany";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostView()
        {
            if (Upload is null) return RedirectToPage();
            var user = await _userManager.GetUserAsync(User);
            var filePath = await _fileUploadService.SaveFile(Upload, User.GetSubjectId());
            Filename = filePath.DbPath;
            var userId = _userManager.GetUserId(User);
            var newPicture = CopyAndSaveProfilePicture(Filename, userId);
            if (Filename != user.ProfilePicture)
                await _userService.SetProfilePictureAsync(userId, newPicture);

            Filename = newPicture;
            return RedirectToPage();
        }

        private void CheckChanges(ApplicationUser user)
        {
            if (Input.Name != user.Name) user.Name = Input.Name;
            if (Input.Surname != user.Surname) user.Surname = Input.Surname;
            if (Input.Description != user.Description) user.Description = Input.Description;
            if (Input.City != user.City) user.City = Input.City;
            if (Input.Github != user.Github) user.Github = Input.Github;
            if (Input.LinkedIn != user.LinkedIn) user.LinkedIn = Input.LinkedIn;
            if (Input.City != user.City) user.City = Input.City;
        }

        private string CopyAndSaveProfilePicture(string imgUrl, string userId)
        {
            return imgUrl == BaseProfilePic ? BaseProfilePic : MovePictureAndGetUrl(imgUrl, userId);
        }

        private string MovePictureAndGetUrl(string imgUrl, string userId)
        {
            if (imgUrl.IsNullOrEmpty()) return BaseProfilePic;

            try
            {
                var path = Path.Combine("Resources", "Images", "Profile");
                return MoveAndGetUrl(imgUrl, userId, path, "Profile");
            }
            catch (IOException e)
            {
                _logger.LogInformation(e, "IOException, reverting to the default image");
                return BaseProfilePic;
            }
        }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Imię")] public string Name { get; set; }

            [Display(Name = "Nazwisko")] public string Surname { get; set; }

            [Display(Name = "Opis")] public string Description { get; set; }

            [Display(Name = "Nazwisko")] public string ProfilePicture { get; set; }


            [Display(Name = "Lokalizacja")] public City City { get; set; }

            [Display(Name = "Link do profilu GitHub")]
            public string Github { get; set; }

            [Display(Name = "Link do profilu LinkedIn")]
            public string LinkedIn { get; set; }

            //TODO: add Skills/Education/Experience module
            public ICollection<Skill> Skills { get; set; }

            public ICollection<Education> Education { get; set; }
            public ICollection<Experience> Experience { get; set; }
        }
    }
}