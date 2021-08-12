namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class CooperativesController : Controller
    {
        private const int DefaultPageSize = 6;

        private readonly ICooperativesService _cooperativesService;
        private readonly IJoinRequestsService _joinRequestsService;

        public CooperativesController(
            ICooperativesService cooperativesService,
            IJoinRequestsService joinRequestsService)
        {
            this._cooperativesService = cooperativesService;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeDetailsViewModel>(id);

            if (currentCooperative == null)
            {
                return this.NotFound();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.GetId();
                currentCooperative.IsAdmin = await this._cooperativesService
                    .CheckIfAdminAsync(userId, currentCooperative.Id);
                currentCooperative.IsMember = await this._cooperativesService
                    .CheckIfMemberAsync(userId, currentCooperative.Id);
                currentCooperative.PendingRequestId = await this._joinRequestsService
                    .GetPendindRequestIdAsync(userId, id);
            }          

            this.ViewData["id"] = currentCooperative.Id;

            return this.View(currentCooperative);
        }

        public async Task<IActionResult> All(FilterViewModel filter, int pageIndex = 1)
        {
            var cooperatives = await this._cooperativesService
                    .GetAllAsync<CooperativeAllViewModel>(
                pageIndex, 
                DefaultPageSize, 
                filter.Level, 
                filter.City, 
                filter.Neighbourhood);

            var count = 0;
            if (cooperatives != null)
            {
                count = await this._cooperativesService
                    .CountAsync(filter.Level, filter.City, filter.Neighbourhood);
            }

            TempData["levelDisplayName"] = "grade";

            var cooperativesList = new CooperativesAllListViewModel
            {
                Cooperatives = PaginatedList<CooperativeAllViewModel>
                    .Create(cooperatives, count, pageIndex, DefaultPageSize),
                Filter = filter
            };

            return this.View(cooperativesList);
        }
    }
}