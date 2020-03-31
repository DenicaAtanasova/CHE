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
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(
            UserManager<CheUser> userManager,
            IJoinRequestsService joinRequestsService)
        {
            this._userManager = userManager;
            this._joinRequestsService = joinRequestsService;
        }

        public IActionResult Send(string cooperativeId, string receiverId)
        {
            return this.View(new JoinRequestCreateInputModel
            {
                CooperativeId = cooperativeId,
                ReceiverId = receiverId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var senderId = this._userManager.GetUserId(this.User);

            var sendRequestSuccessful = await this._joinRequestsService
                .CreateAsync(inputModel.Content, inputModel.CooperativeId, inputModel.ReceiverId, senderId);
            if (!sendRequestSuccessful)
            {
                return this.BadRequest();
            }

            return RedirectToAction("Details", "Cooperatives", new { area = "", id = inputModel.CooperativeId});
        }
    }
}