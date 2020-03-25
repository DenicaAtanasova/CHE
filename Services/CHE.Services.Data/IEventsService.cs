namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string scheduleId, string date);

        Task<bool> CreateAsync(string title, string descrition, DateTime startDate, DateTime endDate, bool isFullDay, string scheduleId);
    }
}