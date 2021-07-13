namespace CHE.Web.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class CooperativesController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 6;

        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;
        private readonly IGradesService _gradesService;
        private readonly IAddressesService _addressesService;

        public CooperativesController(
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService,
            IGradesService gradesService,
            IAddressesService addressesService)
        {
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
            this._gradesService = gradesService;
            this._addressesService = addressesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeDetailsViewModel>(id);

            var userId = this._userManager.GetUserId(this.User);
            this.ViewData["isMember"] = await this._cooperativesService
                .CheckIfMemberAsync(userId, currentCooperative.Id);
            this.ViewData["isAdmin"] = await this._cooperativesService
                .CheckIfAdminAsync(userId, currentCooperative.Id);
            this.ViewData["requestExists"] = await this._cooperativesService
                .CheckIfRequestExistsAsync(id, userId);
            this.ViewData["id"] = currentCooperative.Id;
            this.ViewData["scheduleId"] = currentCooperative.ScheduleId;

            return this.View(currentCooperative);
        }

        public async Task<IActionResult> All(CooperativeAllFilterViewModel filter, int pageIndex = 1)
        {
            var cooperatives = await this._cooperativesService
                    .GetAllAsync<CooperativeAllViewModel>(pageIndex, DEFAULT_PAGE_SIZE, filter.Grade, filter.City, filter.Neighbourhood);
            var count = await this._cooperativesService.CountAsync(filter.Grade, filter.City, filter.Neighbourhood);

            var cooperativesList = new CooperativeAllListViewModel
            {
                Cooperatives = PaginatedList<CooperativeAllViewModel>
                .Create(cooperatives, count, pageIndex, DEFAULT_PAGE_SIZE),
                Filter = filter, 
                Grades = await this._gradesService.GetAllValuesAsync(),
                Cities = await this._addressesService.GetAllCitiesAsync(),
                Neighbourhoods = await this._addressesService.GetAllNeighbourhoodsAsync()
            };

            return this.View(cooperativesList);
        }

        [Authorize]
        public async Task<IActionResult> Leave(string cooperativeId)
        {
            var userId = this._userManager.GetUserId(this.User);
            await this._cooperativesService
                .RemoveMemberAsync(userId, cooperativeId);

            return this.RedirectToAction(nameof(Details), new { id = cooperativeId });
        }
    }
}