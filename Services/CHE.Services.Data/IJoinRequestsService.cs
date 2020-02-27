namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<bool> CreateAsync(string content, string cooperativeId, string senderId, string receiverId);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);
    }
}