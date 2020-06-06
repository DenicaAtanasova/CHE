namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Schedules;

    public class SchedulesController : Controller
    {
        private const string COOPERATIVE_LAYOUT = "/Views/Shared/_LayoutCooperative.cshtml";
        private const string TEACHER_LAYOUT = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";

        private readonly ISchedulesService _schedulesService;

        public SchedulesController(ISchedulesService schedulesService)
        {
            this._schedulesService = schedulesService;
        }

        [Route("scheduler/{id}")]
        public async Task<IActionResult> Details(string id)
        { 
            var schedule = await this._schedulesService.GetByIdAsync<ScheduleViewModel>(id);
            if (schedule.TeacherId == null)
            {
                this.ViewData["layout"] = COOPERATIVE_LAYOUT;
            }
            else
            {
                this.ViewData["layout"] = TEACHER_LAYOUT;
            }
            this.ViewData["id"] = schedule.CooperativeId;
            this.ViewData["scheduleId"] = schedule.Id;

            return View(schedule);
        }
    }
}