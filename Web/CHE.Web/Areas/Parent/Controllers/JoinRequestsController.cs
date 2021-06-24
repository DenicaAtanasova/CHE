namespace CHE.Web.Areas.Parent.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.JoinRequests;

    public class JoinRequestsController : ParentController
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ICheUsersService _usersService;

        public JoinRequestsController(
            UserManager<CheUser> userManager,
            ICheUsersService usersService)
        {
            this._userManager = userManager;
            this._usersService = usersService;
        }

        public IActionResult Send(string cooperativeId, string receiverId)
        {
            return this.View(new JoinRequestInputModel
            {
                CooperativeId = cooperativeId,
                ReceiverId = receiverId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var senderId = this._userManager.GetUserId(this.User);

            await this._usersService
                .SendRequestAsync(senderId, inputModel);

            return RedirectToAction("Details", "Cooperatives", new { area = "", id = inputModel.CooperativeId});
        }
    }
}