namespace CHE.Web.Areas.Parent.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;

    public class CooperativesController : ParentController
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;

        public CooperativesController(
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
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
            var createSuccessful = await this._cooperativesService
                .CreateAsync(model.Name, model.Info, model.Grade, userId, model.Address);
            if (!createSuccessful)
            {
                return this.BadRequest();
            }

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

            var updateSuccessful = await this._cooperativesService
                .UpdateAsync(model.Id, model.Name, model.Info, model.Grade, model.Address);
            if (!updateSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = model.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var deleteSuccessful = await this._cooperativesService
                .DeleteAsync(id);
            if (!deleteSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("All", "Cooperatives", new { area = "" });
        }

        public async Task<IActionResult> RemoveMember(string memberId, string cooperativeId)
        {
            var removeMemberSuccessful = await this._cooperativesService
                .RemoveMemberAsync(memberId, cooperativeId);
            if (!removeMemberSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> Members(string id)
        {
            var cooperativeWithMembers = await this._cooperativesService.GetByIdAsync<CooperativeMembersViewModel>(id);
            this.ViewData["id"] = cooperativeWithMembers.Id;
            this.ViewData["scheduleId"] = cooperativeWithMembers.ScheduleId;

            return this.View(cooperativeWithMembers);
        }

        public async Task<IActionResult> Requests(string id)
        {
            var cooperativeWithRequests = await this._cooperativesService
                .GetByIdAsync<CooperativeJoinRequestsViewModel>(id);
            this.ViewData["id"] = cooperativeWithRequests.Id;
            this.ViewData["scheduleId"] = cooperativeWithRequests.ScheduleId;

            return this.View(cooperativeWithRequests);
        }
    }
}