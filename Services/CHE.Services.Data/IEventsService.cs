namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Events;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string scheduleId, string date);

        Task<string> CreateAsync(EventCreateInputModel inputModel);

        Task DeleteAsync(string id);

        Task UpdateAsync(EventUpdateInputModel inputModel);
    }
}