namespace CHE.Web.ViewComponents
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CooperativesListViewComponent : ViewComponent
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;

        public CooperativesListViewComponent(
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal user)
        {
            var userId = this._userManager.GetUserId(user);
            var cooperativesList = await this._cooperativesService
                .GetAllByAdminAsync<JoinRequestCooperativeSendViewModel>(userId);

            return this.View(cooperativesList);
        }
    }
}