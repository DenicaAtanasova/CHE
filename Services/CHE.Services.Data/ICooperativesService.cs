namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Cooperatives;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICooperativesService
    {
        Task<string> CreateAsync(
            string adminId,
            CooperativeCreateInputModel inputModel);

        Task UpdateAsync(CooperativeUpdateInputModel inputModel);

        Task DeleteAsync(string id);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1,
            int endIndex = 0,
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);

        Task<IEnumerable<TEntity>> GetAllByAdminAsync<TEntity>(
            string userId,
            int startIndex = 1,
            int endIndex = 0);

        Task<IEnumerable<TEntity>> GetAllByAdminOrMemberAsync<TEntity>(
            string userId,
            int startIndex = 1,
            int endIndex = 0);

        Task AddMemberAsync(string userId, string cooperativeId);

        Task RemoveMemberAsync(string memberId, string cooperativeId);

        Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id);

        Task ChangeAdminAsync(string cooperativeId, string userId);

        Task<bool> CheckIfAdminAsync(string userId, string cooperativeId);

        Task<bool> CheckIfMemberAsync(string userId, string cooperativeId);

        Task<bool> CheckIfRequestExistsAsync(string cooperativeId, string senderId);

        Task<int> CountAsync(string userId);

        Task<int> CountAsync(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null);
    }
}