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
        public async Task<ActionResult<EventViewModel>> GetEvent(string id)
        {
            var currentEvent = await this._eventsService.GetByIdAsync<EventViewModel>(id);

            return currentEvent;
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
        public async Task<ActionResult<EventCreateInputModel>> Create(EventCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            await this._eventsService
                .CreateAsync(inputModel);
            
            return this.CreatedAtAction(nameof(GetEvent), inputModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await this._eventsService.DeleteAsync(id);
            return this.NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(EventUpdateInputModel inputModel)
        {
            await this._eventsService.UpdateAsync(inputModel);

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            return this.NoContent();
        }
    }
}