namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Tests.Mocks;
    using CHE.Services.Mapping;
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
            this._dbContext = DatabaseMock.Instance;

            this._eventsService = new EventsService(this._dbContext);

            AutoMapperConfig.RegisterMappings(typeof(EventViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewEvent()
        {
            var title = "Do Sth";
            var description = "Description";
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddMinutes(30);
            var scheduleId = Guid.NewGuid().ToString();

            var eventId = await this._eventsService
                .CreateAsync(title, description, startDate, endDate, scheduleId);

            var eventFromDb = await this._dbContext.Events.SingleOrDefaultAsync();
            var ecpectedCreatedOn = DateTime.UtcNow;

            Assert.Equal(eventId, eventFromDb.Id);
            Assert.Equal(title, eventFromDb.Title);
            Assert.Equal(startDate, eventFromDb.StartDate);
            Assert.Equal(endDate, eventFromDb.EndDate);
            Assert.Equal(scheduleId, eventFromDb.ScheduleId);
            Assert.Equal(ecpectedCreatedOn, 
                eventFromDb.CreatedOn, 
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEvent()
        {
            var scheduleId = Guid.NewGuid().ToString();

            var newEvent = new Event
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(1),
                ScheduleId = scheduleId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();

            var title = "Updated title";
            var description = "Updated description";
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddMinutes(30);

            await this._eventsService
                .UpdateAsync(newEvent.Id, title, description, startDate, endDate);

            var expectedModifiedOnDate = DateTime.UtcNow;
            var updatedEvent = await this._dbContext.Events
                .SingleOrDefaultAsync(x => x.Id == newEvent.Id);

            Assert.Equal(title, updatedEvent.Title);
            Assert.Equal(startDate, updatedEvent.StartDate);
            Assert.Equal(endDate, updatedEvent.EndDate);
            Assert.Equal(expectedModifiedOnDate,
                updatedEvent.ModifiedOn.Value,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEvent()
        {
            var newEvent = new Event
            {
                Title = "Do Sth",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(1),
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();

            await this._eventsService.DeleteAsync(newEvent.Id);

            Assert.Empty(this._dbContext.Events);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEvent()
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
        public async Task GetThreeMonthsEventsAsync_ShouldReturnCorrectEvents()
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
                            x.StartDate.Month < searchedDate.Month + 2)
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