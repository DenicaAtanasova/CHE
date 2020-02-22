using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHE.Services.Data;
using CHE.Web.InputModels.Cooperatives;
using Microsoft.AspNetCore.Mvc;

namespace CHE.Web.Controllers
{
    public class CooperativesController : Controller
    {
        private readonly ICooperativesService _cooperativesService;

        public CooperativesController(ICooperativesService cooperativesService)
        {
            this._cooperativesService = cooperativesService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CooperativeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var username = this.User.Identity.Name;
            await this._cooperativesService.CreateAsync(model.Name, model.Info, model.Grade, username);
            return this.Redirect("/");
        }
    }
}