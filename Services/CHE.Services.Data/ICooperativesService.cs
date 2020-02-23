namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<bool> CreateAsync(string name, string info, string gradeValue, string creatorName);

        Task<bool> DeleteAsync(string id);

        Task UpdateAsync<TEntity>(string id, TEntity entity);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task AddMemberAsync(string cooperativeId, string memberId);

        Task RemoveMemberAsync(string cooperativeId, string memberId);

        Task AcceptRequest(string cooperativeId, string requestId);

        Task RejectRequest(string cooperativeId, string requestId);

        Task SendTeacherRequest(string cooperativeId, string teacherId);

        Task RemoveTeacherRequest(string cooperativeId, string teacherId);
    }
}