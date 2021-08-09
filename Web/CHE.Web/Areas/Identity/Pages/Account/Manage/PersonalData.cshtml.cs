namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using CHE.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;

        public PersonalDataModel(UserManager<CheUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this._userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return this.Page();
        }
    }
}