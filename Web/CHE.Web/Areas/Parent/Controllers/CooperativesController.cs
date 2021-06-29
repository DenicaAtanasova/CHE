namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Linq;
    using System.Threading.Tasks;

    public class CooperativesController : ParentController
    {
        private const int DEFAULT_PAGE_SIZE = 6;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CooperativeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this._userManager.GetUserId(this.User);
             await this._cooperativesService
                .CreateAsync(userId, model);

            return this.RedirectToAction("All", "Cooperatives", new { area = ""});
        }

        public async Task<IActionResult> Update(string id)
        {
            var cooperativeToEdit = await this._cooperativesService
                .GetByIdAsync<CooperativeUpdateInputModel>(id);

            return this.View(cooperativeToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CooperativeUpdateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model.Id);
            }

            await this._cooperativesService.UpdateAsync(model);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = model.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            await this._cooperativesService.DeleteAsync(id);

            return this.RedirectToAction("All", "Cooperatives", new { area = "" });
        }

        public async Task<IActionResult> RemoveMember(string memberId, string cooperativeId)
        {
            await this._cooperativesService
                .RemoveMemberAsync(memberId, cooperativeId);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> Members(string id)
        {
            var cooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeMembersViewModel>(id);
            cooperative.Members = await this._cooperativesService
                .GetMembersAsync<CooperativeUserDetailsViewModel>(id);

            this.ViewData["id"] = cooperative.Id;
            this.ViewData["scheduleId"] = cooperative.ScheduleId;

            return this.View(cooperative);
        }

        public async Task<IActionResult> Requests(string id)
        {
            var cooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeJoinRequestsViewModel>(id);
            cooperative.JoinRequestsReceived = await this._joinRequestsService
                .GetAllByCooperativeAsync<JoinRequestAllViewModel>(id);

            this.ViewData["id"] = cooperative.Id;
            this.ViewData["scheduleId"] = cooperative.ScheduleId;

            return this.View(cooperative);
        }

        public async Task<IActionResult> All(int pageIndex = 1)
        {
            var userId = this._userManager.GetUserId(this.User);
            var cooperatives = await this._cooperativesService
                .GetAllByCreatorAsync<CooperativeAllViewModel>(userId, pageIndex, DEFAULT_PAGE_SIZE);

            var count = await this._cooperativesService.CountAsync(userId);

            var cooperativesList = PaginatedList<CooperativeAllViewModel>
                .Create(cooperatives, count, pageIndex, DEFAULT_PAGE_SIZE);

            return this.View(cooperativesList);
        }
    }
}