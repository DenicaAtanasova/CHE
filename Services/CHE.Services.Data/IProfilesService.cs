namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProfilesService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);

        IEnumerable<string> GetAllSchoolLevels(string currentSchoolLevel);

        Task<string> CreateAsync(string userId);

        Task<bool> UpdateAsync<TEntity>(string userId, TEntity portfolio, IFormFile imageFile);
    }
}