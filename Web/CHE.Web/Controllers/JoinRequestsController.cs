namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;

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
    }
}