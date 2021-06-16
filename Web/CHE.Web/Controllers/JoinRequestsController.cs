namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Web.ViewModels.JoinRequests;
    using CHE.Services.Data;

    public class JoinRequestsController : Controller
    {
        private readonly IJoinRequestsService _joinRequestsService;
        private readonly ICheUsersService _cheUsersService;

        public JoinRequestsController(
            IJoinRequestsService joinRequestsService,
            ICheUsersService cheUsersService)
        {
            this._joinRequestsService = joinRequestsService;
            this._cheUsersService = cheUsersService;
        }

        [Authorize]
        public async Task<IActionResult> TeacherAll(string teacherId)
        {
            var requests = await this._joinRequestsService
                .GetAllByTeacherAsync<JoinRequestAllViewModel>(teacherId);

            return this.View(requests);
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
            await this._cheUsersService.RejectRequestAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }

        [Authorize]
        public async Task<IActionResult> Accept(string cooperativeId, string requestId)
        {
            await this._cheUsersService.AcceptRequestAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }
    }
}