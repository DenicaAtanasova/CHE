﻿namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Common.Extensions;
    using CHE.Services.Data;
    using CHE.Services.Data.Enums;
    using CHE.Web.Cache;
    using CHE.Web.Infrastructure;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using static WebConstants;

    public class CooperativesController : ParentController
    {
        private readonly ICooperativesService _cooperativesService;
        private readonly IAddressCache _addressCache;

        public CooperativesController(
            ICooperativesService cooperativesService,
            IAddressCache addressCache)
        {
            this._cooperativesService = cooperativesService;
            this._addressCache = addressCache;
        }

        public IActionResult Create() => 
            this.View();

        [HttpPost]
        public async Task<IActionResult> Create(CooperativeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.User.GetId();
            await this._cooperativesService.CreateAsync(
                userId, 
                model.Name, 
                model.Info, 
                model.Grade, 
                model.Address.City, 
                model.Address.Neighbourhood);

            this._addressCache.Set(model.Address.City, model.Address.Neighbourhood);

            return this.RedirectToAction("All", "Cooperatives", new { area = ""});
        }

        public async Task<IActionResult> Update(string id)
        {
            var cooperativeToUpdate = await this._cooperativesService
                .GetByIdAsync<CooperativeUpdateInputModel>(id);

            if (cooperativeToUpdate == null)
            {
                return this.NotFound();
            }

            return this.View(cooperativeToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CooperativeUpdateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this._cooperativesService.UpdateAsync(
                model.Id, 
                model.Name, 
                model.Info, 
                model.Grade, 
                model.Address.City, 
                model.Address.Neighbourhood);

            this._addressCache.Set(model.Address.City, model.Address.Neighbourhood);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area = "", id = model.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!id.IsValidString())
            {
                return NotFound();
            }

            await this._cooperativesService.DeleteAsync(id);

            return this.RedirectToAction("All", "Cooperatives", new { area = "" });
        }

        public async Task<IActionResult> MakeAdmin(string cooperativeId, string userId)
        {
            await this._cooperativesService.ChangeAdminAsync(userId, cooperativeId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> RemoveMember(string cooperativeId, string memberId)
        {
            await this._cooperativesService
                .RemoveMemberAsync(memberId, cooperativeId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area = "", id = cooperativeId });
        }

        public async Task<IActionResult> Members(string id)
        {
            var cooperative = await this._cooperativesService
                .GetByIdAsync<CooperativeMembersViewModel>(id);

            if (cooperative == null)
            {
                return this.NotFound();
            }

            this.ViewData["id"] = cooperative.Id;

            return this.View(cooperative);
        }

        public async Task<IActionResult> Leave(string cooperativeId)
        {
            var userId = this.User.GetId();
            await this._cooperativesService
                .RemoveMemberAsync(userId, cooperativeId);

            return this.RedirectToAction(
                "Details", "Cooperatives", new { area="", id = cooperativeId });
        }

        public async Task<IActionResult> MyAll()
        {
            var userId = this.User.GetId();
            var cooperatives = await this._cooperativesService
                    .GetAllByUserAsync<CooperativeAllViewModel>(
                        userId, 
                        CooperativeUserType.Admin | CooperativeUserType.Member);

            return this.View(cooperatives);
        }
    }
}