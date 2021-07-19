namespace CHE.Web.ViewComponents
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CooperativeLayoutNavViewComponent : ViewComponent
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;
        private readonly ICooperativesService _cooperativesService;

        public CooperativeLayoutNavViewComponent(
            UserManager<CheUser> userManager,
            ISchedulesService schedulesService,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._schedulesService = schedulesService;
            this._cooperativesService = cooperativesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal user, string cooperativeId)
        {
            var userId = this._userManager.GetUserId(user);

            return this.View( new CooperativeLayoutNavViewModel
            {
                CooperativeId = cooperativeId,
                IsAdmin = await this._cooperativesService.CheckIfAdminAsync(userId, cooperativeId),
                IsMember = await this._cooperativesService.CheckIfMemberAsync(userId, cooperativeId),
                ScheduleId = await this._schedulesService.GetIdByCooperativeAsync(cooperativeId)
            });
        }
    }
}