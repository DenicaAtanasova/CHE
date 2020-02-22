namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReviewsService
    {
        Task CreateAsync(string comment, string rating, string senderName, string receiverName);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();
    }
}