namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<string> GetPendindRequestIdAsync(string userId, string cooperativeId);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(string cooperativeId);

        Task<string> CreateAsync(
            string senderId,
            string cooperativeId,
            string content);

        Task UpdateAsync(string id, string content);

        Task DeleteAsync(string id);

        Task<bool> ExistsAsync(string senderId, string cooperativeId);
    }
}