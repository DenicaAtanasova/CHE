namespace CHE.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using CHE.Services.Data;
    using CHE.Web.InputModels.JoinRequests;

    public class JoinRequestsController : Controller
    {
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(IJoinRequestsService joinRequestsService)
        {
            this._joinRequestsService = joinRequestsService;
        }

        [Authorize]
        public IActionResult Create(string cooperativeId)
        {
            return View(new JoinRequestCreateInputModel { CooperativeId = cooperativeId});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(JoinRequestCreateInputModel model)
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
    }
}