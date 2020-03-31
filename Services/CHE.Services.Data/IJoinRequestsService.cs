namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJoinRequestsService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetTeacherAllAsync<TEntity>(string teacherId);

        Task<bool> CreateAsync(string requestContent, string cooperativeId, string receiverId, string senderId);

        Task<bool> AcceptAsync(string requestId);

        Task<bool> RejectAsync(string requestId);
    }
}