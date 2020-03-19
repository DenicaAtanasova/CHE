namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface ISchedulesService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);
    }
}