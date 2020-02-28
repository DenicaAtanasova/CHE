namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IPortfoliosService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);
    }
}