namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImagesService
    {
        Task<bool> UpdateAsync(IFormFile imageFile, string portfolioId);
    }
}
