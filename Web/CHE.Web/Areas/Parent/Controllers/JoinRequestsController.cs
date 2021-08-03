namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class JoinRequestsController : ParentController
    {
        private readonly IParentsService _parentsService;
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(
            IParentsService parentsService,
            IJoinRequestsService joinRequestsService)
        {
            this._parentsService = parentsService;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<IActionResult> All(string cooperativeId)
        {
            this.ViewData["id"] = cooperativeId;
            return this.View(new JoinRequestCooperativeAllListViewModel
            {
                CooperativeId = cooperativeId,
                JoinRequests = await this._joinRequestsService
                .GetAllAsync<JoinRequestCooperativeAllViewModel>(cooperativeId)
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

        public IActionResult Send(string cooperativeId) =>
            this.View(new JoinRequestCreateInputModel
            {
                CooperativeId = cooperativeId
            });

        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel.CooperativeId);
            }

            var userId = this.User.GetId();

            await this._parentsService
                .SendRequestAsync(userId, inputModel.CooperativeId, inputModel.Content);

            return RedirectToAction("Details", "Cooperatives", new { area = "", id = inputModel.CooperativeId});
        }

        public async Task<IActionResult> Reject(string requestId, string cooperativeId, string senderId)
        {
            await this._parentsService
                .RejectRequestAsync(senderId, cooperativeId, requestId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> Accept(string requestId, string cooperativeId, string senderId)
        {
            await this._parentsService
                .AcceptRequestAsync(senderId, cooperativeId, requestId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> Update(string id)
        {
            var request = await this._joinRequestsService
                .GetByIdAsync<JoinRequestUpdateInputModel>(id);

            if (request == null)
            {
                return this.NotFound();
            }

            return this.View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Update(JoinRequestUpdateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel.Id);
            }

            await this._joinRequestsService.UpdateAsync(inputModel.Id, inputModel.Content);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = inputModel.CooperativeId });
        }

        public async Task<IActionResult> Delete(string requestId, string cooperativeId)
        {
            await this._joinRequestsService.DeleteAsync(requestId);

            return this.RedirectToAction("Details", "Cooperatives", new { area = "", id = cooperativeId });
        }
    }
}