namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.InputModels.JoinRequests;

    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        [Authorize]
        public IActionResult SendRequest(string cooperativeId)
        {
            return View(new JoinRequestCreateInputModel { CooperativeId = cooperativeId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendRequest(JoinRequestCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var senderName = this.User.Identity.Name;

            var sendRequestSucceeded = await this._usersService
                .SendJoinRequestAsync(model.Content, model.CooperativeId, model.ReceiverId, senderName);
            if (!sendRequestSucceeded)
            {
                //TODO: Make error view
                return this.BadRequest();
            }

            return RedirectToAction("All", "Cooperatives");
        }

        [Authorize]
        public async Task<IActionResult> RejectRequest(string cooperativeId, string requestId)
        {
            var rejectRequestSucceeded = await this._usersService.RejectRequestAsync(requestId);
            if (!rejectRequestSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction( "Details", "Cooperatives", new { id = cooperativeId });
        }

        [Authorize]
        public async Task<IActionResult> AcceptRequest(string cooperativeId, string requestId)
        {
            var acceptRequestSucceeded = await this._usersService.AcceptRequestAsync(requestId);
            if (!acceptRequestSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }

        [Authorize]
        public async Task<IActionResult> RemoveMember(string memberId, string cooperativeId)
        {
            var removeMemberSucceeded = await this._usersService.RemoveMemberFromCooperativeAsync(memberId, cooperativeId);
            if (!removeMemberSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Cooperatives", new { id = cooperativeId });
        }
    }
}