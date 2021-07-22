namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Schedules;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    [Authorize]
    public class SchedulesController : Controller
    {
        private const string COOPERATIVE_LAYOUT = "/Views/Shared/_LayoutCooperative.cshtml";
        private const string ACCOUNT_LAYOUT = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";

        private readonly ISchedulesService _schedulesService;

        public SchedulesController(
            ISchedulesService schedulesService)
        {
            this._schedulesService = schedulesService;
        }

        [Route("Schedule/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<ScheduleViewModel>(id);

            if (schedule == null)
            {
                return this.NotFound();
            }

            this.ViewData["id"] = schedule.CooperativeId;

            if (schedule.CooperativeId == null)
            {
                this.ViewData["layout"] = ACCOUNT_LAYOUT;
            }
            else
            {
                this.ViewData["layout"] = COOPERATIVE_LAYOUT;
            }

            return View(schedule);
        }
    }
}