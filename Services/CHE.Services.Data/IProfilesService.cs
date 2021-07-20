namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Profiles;
    using Microsoft.AspNetCore.Http;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProfilesService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);

        IEnumerable<string> GetAllSchoolLevels();

        Task<string> CreateAsync(string userId);

        Task UpdateAsync(string userId, ProfileInputModel inputModel, IFormFile imageFile);
    }
}