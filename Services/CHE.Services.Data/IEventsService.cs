namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventsService
    {
        Task<IEnumerable<TEntity>> GetThreeMonthsEventsAsync<TEntity>(string date);
    }
}