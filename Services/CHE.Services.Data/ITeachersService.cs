namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeachersService
    {
        Task<string> CreateAsync(string userId);

        Task<TEntity> GetByIdAsync<TEntity>(string id);

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevel = null,
            string city = null,
            string neighbourhood = null);

        Task<int> CountAsync(
            string schoolLevel = null,
            string city = null,
            string neighbourhood = null);
    }
}