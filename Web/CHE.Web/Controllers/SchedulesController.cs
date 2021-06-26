namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Schedules;

    public class SchedulesController : Controller
    {
        private const string COOPERATIVE_LAYOUT = "/Views/Shared/_LayoutCooperative.cshtml";
        private const string TEACHER_LAYOUT = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";

        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;
        private readonly ICooperativesService _cooperativesService;

        public SchedulesController(
            UserManager<CheUser> userManager,
            ISchedulesService schedulesService,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._schedulesService = schedulesService;
            this._cooperativesService = cooperativesService;
        }

        [Route("scheduler/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var schedule = await this._schedulesService.GetByIdAsync<ScheduleViewModel>(id);
            var userId = this._userManager.GetUserId(this.User);

            this.ViewData["isAuthenticated"] = await this._cooperativesService
                                                .CheckIfMemberAsync(userId, schedule.CooperativeId) ||
                                               await this._cooperativesService
                                                .CheckIfCreatorAsync(userId, schedule.CooperativeId);
            this.ViewData["id"] = schedule.CooperativeId;
            this.ViewData["scheduleId"] = schedule.Id;

            if (schedule.TeacherId == null)
            {
                this.ViewData["layout"] = COOPERATIVE_LAYOUT;
            }
            else
            {
                this.ViewData["layout"] = TEACHER_LAYOUT;
            }

            return View(schedule);
        }
    }
}