namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using CHE.Web.InputModels.Portfolios;
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Common;

    [Authorize(Roles = GlobalConstants.TEACHER_ROLE)]
    public class PortfolioModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly IPortfoliosService _portfoliosService;

        public PortfolioModel(
            UserManager<CheUser> userManager,
            IPortfoliosService portfoliosService)
        {
            this._userManager = userManager;
            this._portfoliosService = portfoliosService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public PortfolioInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this._userManager.GetUserId(this.User);
            this.Input = await this._portfoliosService
                .GetByUserIdAsync<PortfolioInputModel>(userId);

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
            var updateSuccessful = await this._portfoliosService.UpdateAsync(userId, Input, Input.Image);

            if (!updateSuccessful)
            {
                return this.Page();

            }
            return RedirectToPage("./Index");
            
        }
    }
}
