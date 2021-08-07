namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly SignInManager<CheUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IReviewsService _reviewsService;
        private readonly IImagesService _imagesService;

        public DeletePersonalDataModel(
            UserManager<CheUser> userManager,
            SignInManager<CheUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IReviewsService reviewsService,
            IImagesService imagesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this._reviewsService = reviewsService;
            this._imagesService = imagesService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            if (User.IsParent())
            {
                await this._reviewsService
                    .SetAllSenderIdToNullByUserAsync(user.Id);
            }

            if (User.IsTeacher())
            {
                await this._imagesService.DeleteAsync(user.Id);
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
