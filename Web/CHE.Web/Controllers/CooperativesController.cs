namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

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

            var username = this.User.Identity.Name;
            await this._cooperativesService.CreateAsync(model.Name, model.Info, model.Grade, username);
            return this.Redirect("/");
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