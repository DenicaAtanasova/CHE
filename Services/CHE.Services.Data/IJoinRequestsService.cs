namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task CreateAsync(string content, string senderId, string receiverId);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetCooperativeAllAsync<TEntity>(string cooperativeId);

        Task<IEnumerable<TEntity>> GetTeacherAllAsync<TEntity>(string teacherId);
    }
}