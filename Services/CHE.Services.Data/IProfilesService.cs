namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Profiles;

    using Microsoft.AspNetCore.Http;

    using System.Threading.Tasks;

    public interface IProfilesService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);

        Task<string> CreateAsync(string userId);

        Task UpdateAsync(string userId, ProfileInputModel inputModel, IFormFile imageFile);
    }
}