﻿namespace CHE.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Events;

    [Route("scheduler/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            this._eventsService = eventsService;
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<Dictionary<string, EventViewModel[]>>> GetThreeMonthsEvents(string date)
        {
            var threeMonthsEvents = await this._eventsService.GetThreeMonthsEventsAsync<EventViewModel>(date);

            var groupedEvents = threeMonthsEvents
                .GroupBy(x => x.StartDate.Date)
                .ToDictionary(x => x.Key.Date.ToString("d-M-yyyy"), x => x.ToArray());

            return groupedEvents;
        }
    }
}