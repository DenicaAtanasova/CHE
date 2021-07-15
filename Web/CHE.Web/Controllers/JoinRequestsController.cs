namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    [Authorize]
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

        public async Task<IActionResult> TeacherAll(string teacherId) =>
            this.View(await this._joinRequestsService
                .GetAllByTeacherAsync<JoinRequestAllViewModel>(teacherId));

        public async Task<IActionResult> Details(string id)
        {
            var request = await this._joinRequestsService
                .GetByIdAsync<JoinRequestDetailsViewModel>(id);

            if (request == null)
            {
                return this.NotFound();
            }

            return this.View(request);
        }

        public async Task<IActionResult> Reject(string cooperativeId, string requestId)
        {
            await this._cheUsersService.RejectRequestAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }

        public async Task<IActionResult> Accept(string cooperativeId, string requestId)
        {
            await this._cheUsersService.AcceptRequestAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }
    }
}