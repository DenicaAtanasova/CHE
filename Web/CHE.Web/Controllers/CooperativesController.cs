namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Services.Data;
    using CHE.Data.Models;

    public class CooperativesController : Controller
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;
        private readonly IJoinRequestsService _joinRequestsService;

        public CooperativesController(
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService,
            IJoinRequestsService joinRequestsService)
        {
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooeprativeDetailsViewModel>(id);

            currentCooperative.JoinRequestsReceived = await this._joinRequestsService
                .GetCooperativeAllAsync<CooperativeJoinRequestDetailsViewModel>(id);

            if (this.User.Identity.Name == currentCooperative.CreatorUserName)
            {
                return this.View("DetailsCreator", currentCooperative);
            }

            return this.View(currentCooperative);
        }

        public async Task<IActionResult> All()
        {
            var cooperativesList = new CooperativeAllListViewModel
            {
                Cooperatives = await this._cooperativesService
                    .GetAllAsync<CooperativeAllViewModel>()
            };

            return this.View(cooperativesList);
        }

        [Authorize]
        public async Task<IActionResult> Leave(string cooperativeId)
        {
            var leaveSuccessful = await this._cooperativesService
                .LeaveAsync(cooperativeId, this.User.Identity.Name);
            if (!leaveSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = cooperativeId });
        }
    }
}