namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.JoinRequests;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class JoinRequestsController : ParentController
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ICheUsersService _usersService;
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(
            UserManager<CheUser> userManager,
            ICheUsersService usersService,
            IJoinRequestsService joinRequestsService)
        {
            this._userManager = userManager;
            this._usersService = usersService;
            this._joinRequestsService = joinRequestsService;
        }

        public IActionResult Send(string cooperativeId, string receiverId) =>
            this.View(new JoinRequestCreateInputModel
            {
                CooperativeId = cooperativeId,
                ReceiverId = receiverId
            });

        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestCreateInputModel inputModel)
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

        public async Task<IActionResult> Update(string id) => 
            this.View(await this._joinRequestsService
                .GetByIdAsync<JoinRequestUpdateInputModel>(id));

        [HttpPost]
        public async Task<IActionResult> Update(JoinRequestUpdateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel.Id);
            }

            await this._joinRequestsService.UpdateAsync(inputModel);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = inputModel.CooperativeId });
        }

        public async Task<IActionResult> Delete(string requestId, string cooperativeId)
        {
            await this._joinRequestsService.DeleteAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = cooperativeId });
        }
    }
}