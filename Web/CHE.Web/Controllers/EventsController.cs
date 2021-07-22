namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Events;
    using CHE.Web.InputModels.Events;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using System.Globalization;

    [Authorize]
    [Route("Schedule/[controller]/[action]")]
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;
        private readonly ISchedulesService _schedulesService;

        public EventsController(
            IEventsService eventsService,
            ISchedulesService schedulesService)
        {
            this._eventsService = eventsService;
            this._schedulesService = schedulesService;
        }

        [HttpGet("{date}")]
        public async Task<ActionResult<Dictionary<string, EventViewModel[]>>> GetThreeMonthsEvents(string date, string scheduleId)
        {
            var threeMonthsEvents = await this._eventsService.GetThreeMonthsEventsAsync<EventViewModel>(scheduleId, date);

            var groupedEvents = threeMonthsEvents
                .GroupBy(x => x.StartDate.Date)
                .ToDictionary(x => x.Key.Date.ToString("d-M-yyyy"), x => x.ToArray());

            return this.Json(groupedEvents);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this._eventsService.DeleteAsync(id);
            return this.NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(string id)
        {
            var currentEvent = await this._eventsService.GetByIdAsync<EventUpdateInputModel>(id);
            return this.View(currentEvent);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update(string id, EventUpdateInputModel inputModel)
        {
            await this._eventsService.UpdateAsync(id, inputModel);

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Schedules", new { id = inputModel.ScheduleId});
        }

        public async Task<IActionResult> Add(string scheduleId)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<EventScheduleViewModel>(scheduleId);

            return this.View(new EventCreateInputModel
            {
                ScheduleId = scheduleId,
                CooperativeId = schedule.CooperativeId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            await this._eventsService.CreateAsync(inputModel);

            return this.RedirectToAction("Details", "Schedules", new { id = inputModel.ScheduleId});
        }
    }
}