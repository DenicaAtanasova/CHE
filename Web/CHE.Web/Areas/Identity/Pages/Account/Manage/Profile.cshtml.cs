namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using CHE.Common;
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.Portfolios;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.TEACHER_ROLE)]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly IProfilesService _profilesService;

        public ProfileModel(
            UserManager<CheUser> userManager,
            IProfilesService profilesService)
        {
            this._userManager = userManager;
            this._profilesService = profilesService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this._userManager.GetUserId(this.User);
            this.Input = await this._profilesService
                .GetByUserIdAsync<ProfileInputModel>(userId);

            if (this.Input == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var userId = this._userManager.GetUserId(this.User);
            await this._profilesService.UpdateAsync(userId, Input, Input.Image);

            return RedirectToPage("./Index");            
        }
    }
}
