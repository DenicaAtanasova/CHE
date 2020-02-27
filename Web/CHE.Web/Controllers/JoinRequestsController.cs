﻿namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;
    using CHE.Web.InputModels.JoinRequests;

    public class JoinRequestsController : Controller
    {
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsController(
            IJoinRequestsService joinRequestsService)
        {
            this._joinRequestsService = joinRequestsService;
        }

        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var request = await this._joinRequestsService.GetByIdAsync<JoinRequestDetailsViewModel>(id);

            return this.View(request);
        }

        [Authorize]
        public IActionResult Send(string cooperativeId)
        {
            return View(new JoinRequestCreateInputModel { CooperativeId = cooperativeId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(JoinRequestCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var senderName = this.User.Identity.Name;

            var sendRequestSuccessful = await this._joinRequestsService
                .SendAsync(model.Content, model.CooperativeId, model.ReceiverId, senderName);
            if (!sendRequestSuccessful)
            {
                return this.BadRequest();
            }

            return RedirectToAction("All", "Cooperatives");
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