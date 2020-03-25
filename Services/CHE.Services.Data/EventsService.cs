namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

    public class EventsService : IEventsService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventsService(
            CheDbContext dbContext,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string date)
        {
            var currentDate = DateTime.ParseExact(date, "d-M-yyyy", CultureInfo.InvariantCulture);
            var prevMonth = currentDate.Month - 2;
            var nextMonth = currentDate.Month + 2;
            var year = currentDate.Year;

            var eventsFromDb = await this._dbContext.Events
                .Where(x => x.StartDate.Year == year && x.StartDate.Month > prevMonth && x.StartDate.Month < nextMonth)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
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
    }
}