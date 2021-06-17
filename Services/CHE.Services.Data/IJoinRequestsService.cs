namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllByTeacherAsync<TEntity>(string teacherId);

        Task<IEnumerable<TEntity>> GetAllByCooperativeAsync<TEntity>(string cooperativeId);

        Task<string> CreateAsync(string requestContent, string cooperativeId, string senderId, string receiverId = null);
    }
}