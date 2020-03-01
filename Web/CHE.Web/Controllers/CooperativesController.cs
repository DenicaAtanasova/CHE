namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using CHE.Services.Data;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Data.Models;

    public class CooperativesController : Controller
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

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CooperativeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this._userManager.GetUserId(this.User);
            var createSuccessful = await this._cooperativesService.CreateAsync(model.Name, model.Info, model.Grade, userId);
            if (!createSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var cooperativeToEdit = await this._cooperativesService.GetByIdAsync<CooperativeEditInputModel>(id);

            return this.View(cooperativeToEdit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CooperativeEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model.Id);
            }

            var updateSuccessful = await this._cooperativesService
                .UpdateAsync(model.Id, model.Name, model.Info, model.Grade, model.Address.City, model.Address.Neighbourhood, model.Address.Street);
            if (!updateSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var deleteSuccessful = await this._cooperativesService.DeleteAsync(id);

            if (!deleteSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var currentCooperative = await this._cooperativesService
                .GetByIdAsync<CooeprativeDetailsViewModel>(id);
            currentCooperative.JoinRequestsReceived = await this._cooperativesService
                .GetJoinRequestsAsync<CooperativeJoinRequestDetailsViewModel>(id);

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
                Cooperatives = await this._cooperativesService.GetAllAsync<CooperativeAllViewModel>()
            };

            return this.View(cooperativesList);
        }

        [Authorize]
        public async Task<IActionResult> RemoveMember(string memberId, string cooperativeId)
        {
            var removeMemberSuccessful = await this._cooperativesService.RemoveMemberAsync(memberId, cooperativeId);
            if (!removeMemberSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = cooperativeId });
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