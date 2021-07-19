namespace CHE.Web.ViewComponents
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AccountNavViewComponent : ViewComponent
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;

        public AccountNavViewComponent(
            UserManager<CheUser> userManager, 
            ISchedulesService schedulesService)
        {
            this._userManager = userManager;
            this._schedulesService = schedulesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal user)
        {
            var userId = this._userManager.GetUserId(user);

            return this.View( new TeacherAccountNavViewModel
            {
                ScheduleId = await this._schedulesService.GetIdByUserAsync(userId)
            });
        }
    }
}