﻿namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventsService : IEventsService
    {
        private readonly CheDbContext _dbContext;

        public EventsService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            =>await this._dbContext.Events
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(
            string scheduleId, 
            string date)
        {
            var currentDate = DateTime.ParseExact(date, "d-M-yyyy", CultureInfo.InvariantCulture);
            var prevMonth = currentDate.Month - 2;
            var nextMonth = currentDate.Month + 2;
            var year = currentDate.Year;

            var eventsFromDb = await this._dbContext.Events
                .AsNoTracking()
                .Where(x => x.ScheduleId == scheduleId &&
                            x.StartDate.Year == year &&
                            x.StartDate.Month > prevMonth &&
                            x.StartDate.Month < nextMonth)
                .To<TEntity>()
                .ToListAsync();

            return eventsFromDb;
        } 

        public async Task<string> CreateAsync(
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            string scheduleId)
        {
            var newEvent = new Event
            {
                Title = title,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                ScheduleId = scheduleId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Events.Add(newEvent);
            await this._dbContext.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var eventToDelete = await this._dbContext.Events
                .SingleOrDefaultAsync(x => x.Id == id);

            if (eventToDelete == null)
            {
                return;
            }

            this._dbContext.Remove(eventToDelete);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(
            string id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate)
        {
            var eventToUpdate = await this._dbContext.Events
                .SingleOrDefaultAsync(x => x.Id == id);

            if (eventToUpdate == null)
            {
                return;
            }

            eventToUpdate.Title = title;
            eventToUpdate.Description = description;
            eventToUpdate.StartDate = startDate;
            eventToUpdate.EndDate = endDate;
            eventToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Events.Update(eventToUpdate);
            await this._dbContext.SaveChangesAsync();
        }
    }
}