namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<bool> CreateAsync(string name, string info, string gradeValue, string creatorId);

        Task<bool> UpdateAsync<TAddress>(string cooperativeId, string name, string info, string gradeValue, TAddress addres);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task<IEnumerable<TEntity>> GetCreatorAllByUsernameAsync<TEntity>(string username);

        Task<IEnumerable<TEntity>> GetJoinRequestsAsync<TEntity>(string cooperativeId);

        Task<bool> AddMemberAsync(string userId, string cooperativeId);

        Task<bool> RemoveMemberAsync(string memberId, string cooperativeId);

        Task<bool> LeaveAsync(string cooperativeId, string username);
    }
}