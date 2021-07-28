namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<string> GetPendindRequestIdAsync(string cooperativeId, string senderId);

        Task<IEnumerable<TEntity>> GetAllByTeacherAsync<TEntity>(string teacherId);

        Task<IEnumerable<TEntity>> GetAllByCooperativeAsync<TEntity>(string cooperativeId);

        Task<string> CreateAsync(
            string senderId,
            string content,
            string cooperativeId,
            string receiverId);

        Task UpdateAsync(string id, string content);

        Task DeleteAsync(string id);

        Task<bool> ExistsAsync(string cooperativeId, string senderId, string receiverId);
    }
}