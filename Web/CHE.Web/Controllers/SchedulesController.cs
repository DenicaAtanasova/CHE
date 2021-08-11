namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Schedules;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using static WebConstants;

    [Authorize]
    [Route("/Schedule/[Action]")]
    public class SchedulesController : Controller
    {
        private readonly ISchedulesService _schedulesService;

        public SchedulesController(
            ISchedulesService schedulesService)
        {
            this._schedulesService = schedulesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<ScheduleViewModel>(id);

            if (schedule == null)
            {
                return this.NotFound();
            }

            this.ViewData["id"] = schedule.CooperativeId;
            this.ViewData["layout"] = CooperativeLayout;

            return View(schedule);
        }

        public async Task<IActionResult> MyDetails(string id)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<ScheduleViewModel>(id);

            if (schedule == null)
            {
                return this.NotFound();
            }
                
            this.ViewData["layout"] = AccountLayout;

            return View("Details", schedule);
        }
    }
}