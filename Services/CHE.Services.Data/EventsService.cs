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
    }
}