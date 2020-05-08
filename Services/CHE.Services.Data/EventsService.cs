namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class EventsService : IEventsService
    {
        private readonly CheDbContext _dbContext;

        public EventsService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var eventFromDb = await this._dbContext.Events
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return eventFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string scheduleId, string date)
        {
            var currentDate = DateTime.ParseExact(date, "d-M-yyyy", CultureInfo.InvariantCulture);
            var prevMonth = currentDate.Month - 2;
            var nextMonth = currentDate.Month + 2;
            var year = currentDate.Year;

            var eventsFromDb = await this._dbContext.Events
                .Where(x => x.ScheduleId == scheduleId)
                .Where(x => x.StartDate.Year == year && x.StartDate.Month > prevMonth && x.StartDate.Month < nextMonth)
                .To<TEntity>()
                .ToArrayAsync();

            return eventsFromDb;
        } 

        public async Task<bool> CreateAsync(string title, string descrition, DateTime startDate, DateTime endDate, bool isFullDay, string scheduleId)
        {
            var newEvent = new Event
            {
                Title = title,
                Description = descrition,
                StartDate = startDate,
                EndDate = endDate,
                ScheduleId = scheduleId,
                IsFullDay = isFullDay,
                CreatedOn = DateTime.UtcNow
            };

            await this._dbContext.Events.AddAsync(newEvent);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var eventToDelete = await this._dbContext.Events.SingleOrDefaultAsync(x => x.Id == id);
            this._dbContext.Remove(eventToDelete);

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> UpdateAsync(string id, string title, string descrition, DateTime startDate, DateTime endDate, bool isFullDay)
        {
            var eventToUpdate = await this._dbContext.Events
               .SingleOrDefaultAsync(x => x.Id == id);

            eventToUpdate.Title = title;
            eventToUpdate.Description = descrition;
            eventToUpdate.StartDate = startDate;
            eventToUpdate.EndDate = endDate;
            eventToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Update(eventToUpdate);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }
    }
}