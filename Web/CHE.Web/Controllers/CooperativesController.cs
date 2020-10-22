namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Services.Data;
    using CHE.Data.Models;

    public class CooperativesController : Controller
    {
        //TODO: Change to 18
        private const int DEFAULT_PAGE_SIZE = 18;

        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;

        public CooperativesController(
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeDetailsViewModel>(id);

            var userId = this._userManager.GetUserId(this.User);
            var isMember = await this._cooperativesService.CheckIfMemberAsync(userId, currentCooperative.Id);
            this.ViewData["isMember"] = isMember;
            this.ViewData["id"] = currentCooperative.Id;
            this.ViewData["scheduleId"] = currentCooperative.ScheduleId;

            if (this.User.Identity.Name == currentCooperative.CreatorUserName)
            {
                return this.View("DetailsCreator", currentCooperative);
            }

            return this.View(currentCooperative);
        }

        public async Task<IActionResult> All(CooperativeAllFilterViewModel filter, int pageIndex = 1)
        {
            var cooperatives = await this._cooperativesService
                    .GetAllAsync<CooperativeAllViewModel>(pageIndex, DEFAULT_PAGE_SIZE, filter.Grade);
            var count = await this._cooperativesService.Count();
            var cooperativesList = new CooperativeAllLIstViewModel
            {
                Cooperatives = PaginatedList<CooperativeAllViewModel>
                .Create(cooperatives, count, pageIndex, DEFAULT_PAGE_SIZE)
            };

            return this.View(cooperativesList);
        }

        [Authorize]
        public async Task<IActionResult> Leave(string cooperativeId)
        {
            var userId = this._userManager.GetUserId(this.User);
            var leaveSuccessful = await this._cooperativesService
                .RemoveMemberAsync(cooperativeId, userId);
            if (!leaveSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = cooperativeId });
        }
    }
}