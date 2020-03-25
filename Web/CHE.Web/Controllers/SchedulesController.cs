namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Schedules;

    public class SchedulesController : Controller
    {
        private readonly ISchedulesService _schedulesService;

        public SchedulesController(ISchedulesService schedulesService)
        {
            this._schedulesService = schedulesService;
        }

        [Route("scheduler/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var schedule = await this._schedulesService.GetByIdAsync<ScheduleViewModel>(id);

            return View(schedule);
        }
    }
}