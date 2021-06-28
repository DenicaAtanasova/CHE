namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Events;
    using CHE.Web.ViewModels.Events;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class EventsServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IEventsService _eventsService;

        public EventsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
            this._dbContext = new CheDbContext(options);

            this._eventsService = new EventsService(this._dbContext);

            AutoMapperConfig.RegisterMappings(
                typeof(EventCreateInputModel).Assembly,
                typeof(EventViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var newEvent = new EventCreateInputModel
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMinutes(30),
                ScheduleId = Guid.NewGuid().ToString()
            };

            var eventId = await this._eventsService.CreateAsync(newEvent);
            var eventFromDb = await this._dbContext.Events.SingleOrDefaultAsync();
            var ecpectedCreatedOn = DateTime.UtcNow;

            Assert.Equal(eventId, eventFromDb.Id);
            Assert.Equal(newEvent.Title, eventFromDb.Title);
            Assert.Equal(newEvent.StartDate, eventFromDb.StartDate);
            Assert.Equal(newEvent.EndDate, eventFromDb.EndDate);
            Assert.Equal(newEvent.ScheduleId, eventFromDb.ScheduleId);
            Assert.Equal(ecpectedCreatedOn, eventFromDb.CreatedOn, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var schduleId = Guid.NewGuid().ToString();

            var newEvent = new Event
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(1),
                ScheduleId = schduleId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();
            this._dbContext.Entry(newEvent).State = EntityState.Detached;

            var eventUpdateModel = new EventUpdateInputModel
            {
                Id = newEvent.Id,
                Title = "Updated title",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMinutes(30),
                ScheduleId = schduleId,
                CreatedOn = newEvent.CreatedOn
            };

            await this._eventsService.UpdateAsync(eventUpdateModel);
            var expectedModifiedOnDate = DateTime.UtcNow;
            var updatedEvent = await this._dbContext.Events
                .SingleOrDefaultAsync(x => x.Id == newEvent.Id);

            Assert.Equal(eventUpdateModel.Title, updatedEvent.Title);
            Assert.Equal(eventUpdateModel.StartDate, updatedEvent.StartDate);
            Assert.Equal(eventUpdateModel.EndDate, updatedEvent.EndDate);
            Assert.Equal(eventUpdateModel.ScheduleId, updatedEvent.ScheduleId);
            Assert.Equal(expectedModifiedOnDate,
                updatedEvent.ModifiedOn.Value,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            var newEvent = new Event
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(1),
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();
            this._dbContext.Entry(newEvent).State = EntityState.Detached;

            await this._eventsService.DeleteAsync(newEvent.Id);

            Assert.Empty(this._dbContext.Events);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldWorkCorrectly()
        {
            var newEvent = new Event
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();

            var actualEvent = await this._eventsService
                .GetByIdAsync<EventViewModel>(newEvent.Id);

            Assert.Equal(newEvent.Id, actualEvent.Id);
        }

        [Fact]
        public async Task GetThreeMonthsEventsAsync_ShouldWorkCorrectly()
        {
            var scheduleId = Guid.NewGuid().ToString();

            var events = new List<Event>{
                new Event
                {
                    Title = "Event1",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(30),
                    ScheduleId = scheduleId
                },
                new Event
                {
                    Title = "Event2",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(30),
                    ScheduleId = scheduleId
                },
                new Event
                {
                    Title = "Event3",
                    StartDate = DateTime.UtcNow.AddMonths(1),
                    EndDate = DateTime.UtcNow.AddMonths(1).AddMinutes(30),
                    ScheduleId = scheduleId
                },
                new Event
                {
                    Title = "Event4",
                    StartDate = DateTime.UtcNow.AddMonths(2),
                    EndDate = DateTime.UtcNow.AddMonths(2).AddMinutes(30),
                    ScheduleId = scheduleId
                },
                new Event
                {
                    Title = "Event5",
                    StartDate = DateTime.UtcNow.AddMonths(-1),
                    EndDate = DateTime.UtcNow.AddMonths(-1).AddMinutes(30),
                    ScheduleId = scheduleId
                },
                new Event
                {
                    Title = "Event6",
                    StartDate = DateTime.UtcNow.AddMonths(-2),
                    EndDate = DateTime.UtcNow.AddMonths(-2).AddMinutes(30),
                    ScheduleId = scheduleId
                },
            };

            this._dbContext.Events.AddRange(events);
            await this._dbContext.SaveChangesAsync();
            var searchedDate = DateTime.UtcNow;

            var threeMonthEvents = await this._eventsService
                .GetThreeMonthsEventsAsync<EventViewModel>(
                    scheduleId, 
                    searchedDate.ToString("d-M-yyyy", CultureInfo.InvariantCulture));

            var expectedEvents = events
                .Where(x => x.ScheduleId == scheduleId &&
                            x.StartDate.Year == searchedDate.Year &&
                            x.StartDate.Month > searchedDate.Month - 2 &&
                            x.StartDate.Month < searchedDate.Month +2)
                .ToList();

            Assert.Equal(expectedEvents.Count, threeMonthEvents.Count());

            var index = 0;
            foreach (var eventUnit in threeMonthEvents)
            {
                Assert.Equal(expectedEvents[index++].Id, eventUnit.Id);
            }
        }
    }
}