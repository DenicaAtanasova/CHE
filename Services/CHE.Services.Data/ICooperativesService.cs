namespace CHE.Services.Data
{
    using CHE.Services.Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<string> CreateAsync(
            string adminId,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood);

        Task UpdateAsync(
            string id,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1,
            int endIndex = 0,
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task<IEnumerable<TEntity>> GetAllByUserAsync<TEntity>(
            string userId,
            CooperativeUser userType,
            int startIndex = 1,
            int endIndex = 0);

        Task AddMemberAsync(string userId, string cooperativeId);

        Task RemoveMemberAsync(string memberId, string cooperativeId);

        Task ChangeAdminAsync(string userId, string cooperativeId);

        Task<bool> CheckIfAdminAsync(string userId, string cooperativeId);

        Task<bool> CheckIfMemberAsync(string userId, string cooperativeId);

        Task<int> CountByUserAsync(string userId);

        Task<int> CountAsync(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);
    }
}