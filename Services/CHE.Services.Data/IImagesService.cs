namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImagesService
    {
        Task<string> UpdateAsync(IFormFile imageFile, string portfolioId);

        Task<string> CreateAvatarAsync(string portfolioId);
    }
}