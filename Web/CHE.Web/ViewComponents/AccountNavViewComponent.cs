namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class AccountNavViewComponent : ViewComponent
    {
        private readonly ISchedulesService _schedulesService;

        public AccountNavViewComponent( ISchedulesService schedulesService)
        {
            this._schedulesService = schedulesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            return this.View( new TeacherAccountNavViewModel
            {
                ScheduleId = await this._schedulesService.GetIdByUserAsync(userId)
            });
        }
    }
}