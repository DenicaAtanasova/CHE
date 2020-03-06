namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using CHE.Web.ViewModels.JoinRequests;
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Services.Data;
    using CHE.Data.Models;

    public class JoinRequestsController : Controller
    {
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(
            IJoinRequestsService joinRequestsService)
        {
            this._joinRequestsService = joinRequestsService;
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var request = await this._joinRequestsService.GetByIdAsync<JoinRequestDetailsViewModel>(id);

            return this.View(request);
        }

        [Authorize]
        public async Task<IActionResult> Reject(string cooperativeId, string requestId)
        {
            var rejectRequestSuccessful = await this._joinRequestsService.RejectAsync(requestId);
            if (!rejectRequestSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }

        [Authorize]
        public async Task<IActionResult> Accept(string cooperativeId, string requestId)
        {
            var acceptRequestSuccessful = await this._joinRequestsService.AcceptAsync(requestId);
            if (!acceptRequestSuccessful)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }
    }
}