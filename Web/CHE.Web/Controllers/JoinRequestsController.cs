namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Web.ViewModels.JoinRequests;

    public class JoinRequestsController : Controller
    {
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(IJoinRequestsService joinRequestsService)
        {
            this._joinRequestsService = joinRequestsService;
        }

        [Authorize]
        public IActionResult Send(string cooperativeId)
        {
            return View(new JoinRequestCreateInputModel { CooperativeId = cooperativeId});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var senderName = this.User.Identity.Name;

            var sendRequestSucceeded = await this._joinRequestsService.CreateAsync(model.Content, model.CooperativeId, senderName);
            if (!sendRequestSucceeded)
            {
                return this.BadRequest();
            }

            return RedirectToAction("All", "Cooperatives");
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var request = await this._joinRequestsService.GetByIdAsync<JoinRequestDetailsViewModel>(id);

            return this.View(request);
        }
    }
}