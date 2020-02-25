namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using CHE.Services.Data;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;

    public class CooperativesController : Controller
    {
        private readonly ICooperativesService _cooperativesService;

        public CooperativesController(ICooperativesService cooperativesService)
        {
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

            var username = this.User.Identity.Name;
            var createSucceeded = await this._cooperativesService.CreateAsync(model.Name, model.Info, model.Grade, username);
            if (!createSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string? id)
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

            var updateSucceeded = await this._cooperativesService
                .UpdateAsync(model.Id, model.Name, model.Info, model.Grade, model.Address.City, model.Address.Neighbourhood, model.Address.Street);
            if (!updateSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var deleteSucceeded = await this._cooperativesService.DeleteAsync(id);

            if (!deleteSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var cooperative = await this._cooperativesService
                .GetByIdAsync<CooeprativeDetailsViewModel>(id);

            return this.View(cooperative);
        }

        public async Task<IActionResult> All()
        {
            var cooperativesList = new CooperativeAllListViewModel
            {
                Cooperatives = await this._cooperativesService.GetAllAsync<CooperativeAllViewModel>()
            };

            return this.View(cooperativesList);
        }
    }
}