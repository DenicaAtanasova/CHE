namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using CHE.Common;
    using CHE.Services.Data;
    using CHE.Web.Cache;
    using CHE.Web.Infrastructure;
    using CHE.Web.InputModels.Profiles;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.TeacherRole)]
    public class ProfileModel : PageModel
    {
        private readonly IProfilesService _profilesService;
        private readonly IAddressCache _addressCache;

        public ProfileModel(IProfilesService profilesService, IAddressCache addressCache)
        {
            this._profilesService = profilesService;
            this._addressCache = addressCache;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this.User.GetId();
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

            await this._profilesService.UpdateAsync(
                Input.Id, 
                Input.FirstName,
                Input.LastName,
                Input.Education,
                Input.Experience,
                Input.Skills,
                Input.Interests,
                Input.SchoolLevel,
                Input.Address.City,
                Input.Address.Neighbourhood,
                Input.Image?.OpenReadStream());

            this._addressCache.Set(Input.Address.City, Input.Address.Neighbourhood);
            return RedirectToPage("./Index");  
        }
    }
}