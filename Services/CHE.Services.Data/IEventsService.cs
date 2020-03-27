namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string scheduleId, string date);

        Task<bool> CreateAsync(string title, string descrition, DateTime startDate, DateTime endDate, bool isFullDay, string scheduleId);

        Task<bool> DeleteAsync(string id);

        Task<bool> UpdateAsync(string id, string title, string descrition, DateTime startDate, DateTime endDate, bool isFullDay);
    }
}