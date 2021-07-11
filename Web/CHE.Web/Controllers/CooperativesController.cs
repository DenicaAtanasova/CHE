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
            if (id == null)
            {
                return this.NotFound();
            }

            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeDetailsViewModel>(id);

            var userId = this._userManager.GetUserId(this.User);
            var isMember = await this._cooperativesService.CheckIfMemberAsync(userId, currentCooperative.Id);
            var isCreator = await this._cooperativesService.CheckIfCreatorAsync(userId, currentCooperative.Id);

            this.ViewData["isMember"] = isMember;
            this.ViewData["isCreator"] = isCreator;
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