namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class CooperativeLayoutNavViewComponent : ViewComponent
    {
        private readonly ISchedulesService _schedulesService;
        private readonly ICooperativesService _cooperativesService;

        public CooperativeLayoutNavViewComponent(
            ISchedulesService schedulesService,
            ICooperativesService cooperativesService)
        {
            this._schedulesService = schedulesService;
            this._cooperativesService = cooperativesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, string cooperativeId)
        {
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