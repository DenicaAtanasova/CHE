﻿namespace CHE.Web.Controllers
{
    using CHE.Common.Extensions;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Events;
    using CHE.Web.InputModels.Events;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using CHE.Web.Infrastructure;

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

        [HttpGet("{scheduleId}/{date}")]
        public async Task<IActionResult> GetThreeMonthsEvents(string scheduleId, string date)
        {
            var threeMonthsEvents = await this._eventsService
                .GetThreeMonthsEventsAsync<EventViewModel>(scheduleId, date);

            var groupedEvents = threeMonthsEvents
                .GroupBy(x => x.StartDate.Date)
                .ToDictionary(x => x.Key.Date.ToString("d-M-yyyy"), x => x.ToArray());

            return this.Json(groupedEvents);
        }

        public async Task<IActionResult> Delete(string id, string scheduleId)
        {
            if (!id.IsValidString())
            {
                return NotFound();
            }

            await this._eventsService.DeleteAsync(id);

            if (this.User.IsParent())
            {
                return this.RedirectToAction(
                    "Details", "Schedules", new { id = scheduleId });
            }
            else
            { 
                return this.RedirectToAction(
                    "MyDetails", "Schedules", new { id = scheduleId });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var currentEvent = await this._eventsService
                .GetByIdAsync<EventUpdateInputModel>(id);

            if (currentEvent == null)
            {
                return NotFound();
            }

            return this.View(currentEvent);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update(string id, EventUpdateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(id, inputModel);
            }

            await this._eventsService.UpdateAsync(
                id, 
                inputModel.Title,
                inputModel.Description,
                inputModel.StartDate,
                inputModel.EndDate);

            return this.RedirectToAction("Details", "Schedules", new { id = inputModel.ScheduleId});
        }

        [HttpGet("{scheduleId}/{currentDate}")]
        public async Task<IActionResult> Add(string scheduleId, string currentDate)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<EventScheduleViewModel>(scheduleId);

            if (schedule == null)
            {
                return NotFound();
            }

            var date = DateTime.ParseExact(currentDate, "d-M-yyyy", CultureInfo.InvariantCulture);
            return this.View(new EventCreateInputModel
            {
                ScheduleId = scheduleId,
                CooperativeId = schedule.CooperativeId,
                StartDate = date,
                EndDate = date
            });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(EventCreateInputModel inputModel)
        {
            var schedule = await this._schedulesService
                .GetByIdAsync<EventScheduleViewModel>(inputModel.ScheduleId);

            if (schedule == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this._eventsService.CreateAsync(
                inputModel.Title, 
                inputModel.Description, 
                inputModel.StartDate, 
                inputModel.EndDate, 
                inputModel.ScheduleId);

            return this.RedirectToAction(
                "Details", "Schedules", new { id = inputModel.ScheduleId});
        }
    }
}