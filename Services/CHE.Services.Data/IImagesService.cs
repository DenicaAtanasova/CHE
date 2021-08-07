namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImagesService
    {
        Task UpdateAsync(IFormFile imageFile, string portfolioId);

        Task<string> CreateAvatarAsync(string portfolioId);

        Task DeleteAsync(string userId);
    }
}