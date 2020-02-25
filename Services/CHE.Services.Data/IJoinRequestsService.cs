namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<bool> CreateAsync(string content, string cooperativeId, string senderName);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<ICollection<TEntity>> GetAllByCooperativeId<TEntity>(string cooperativeId);
    }
}