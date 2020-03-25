namespace CHE.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Events;
    using CHE.Web.InputModels.Events;

    [Route("scheduler/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            this._eventsService = eventsService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetEvent(string id)
        {
            return "";
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<Dictionary<string, EventViewModel[]>>> GetThreeMonthsEvents(string date)
        {
            var scheduleId = this.Request.Headers["x-scheduleId"].ToString();

            var threeMonthsEvents = await this._eventsService.GetThreeMonthsEventsAsync<EventViewModel>(scheduleId, date);

            var groupedEvents = threeMonthsEvents
                .GroupBy(x => x.StartDate.Date)
                .ToDictionary(x => x.Key.Date.ToString("d-M-yyyy"), x => x.ToArray());

            return groupedEvents;
        }

        [HttpPost]
        public async Task<ActionResult<EventCreateInputModel>> AddEvent(EventCreateInputModel inputEvent)
        {
            var createSuccessful = await this._eventsService
                .CreateAsync(inputEvent.Title, inputEvent.Description, inputEvent.StartDate, inputEvent.EndDate, inputEvent.IsFullDay, inputEvent.ScheduleId);

            if (!createSuccessful)
            {
                return this.StatusCode(400);
            }
            
            return this.CreatedAtAction(nameof(GetEvent), inputEvent);
        }
    }
}