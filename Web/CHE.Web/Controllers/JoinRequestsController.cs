namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
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

        public async Task<IActionResult> All()
        {
            var userId = this.User.GetId();
            return this.View(new JoinRequestTeacherAllListVIewModel
            {
                JoinRequests = await this._joinRequestsService
                .GetAllByTeacherAsync<JoinRequestTeacherAllVIewModel>(userId)
            });
        }

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

        public async Task<IActionResult> Reject(string requestId, string cooperativeId, string senderId, string receiverId)
        {
            await this._cheUsersService
                .RejectRequestAsync(requestId, cooperativeId, senderId, receiverId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { id = cooperativeId });
        }

        public async Task<IActionResult> Accept(string requestId, string cooperativeId, string senderId, string receiverId)
        {
            await this._cheUsersService
                .AcceptRequestAsync(requestId, cooperativeId, senderId, receiverId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { id = cooperativeId });
        }
    }
}