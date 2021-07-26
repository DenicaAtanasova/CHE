namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string scheduleId, string date);

        Task<string> CreateAsync(
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            string scheduleId);

        Task DeleteAsync(string id);

        Task UpdateAsync(
            string id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate);
    }
}