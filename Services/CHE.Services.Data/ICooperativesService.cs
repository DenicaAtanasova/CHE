namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<bool> CreateAsync<TAddress>(string name, string info, string gradeValue, string creatorId, TAddress addres);

        Task<bool> UpdateAsync<TAddress>(string cooperativeId, string name, string info, string gradeValue, TAddress addres);

        Task<bool> DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(int startIndex, int endIndex);

        Task<IEnumerable<TEntity>> GetCreatorAllByUsernameAsync<TEntity>(string username);

        Task<bool> AddMemberAsync(string userId, string cooperativeId);

        Task<bool> RemoveMemberAsync(string memberId, string cooperativeId);

        Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id);

        Task<bool> CheckIfMemberAsync(string username, string cooperativeId);

        Task<bool> CheckIfCreatorAsync(string username, string cooperativeId);

        Task<IEnumerable<TEntity>> GetRequestsAsync<TEntity>(string id);

        Task<int> Count();
    }
}