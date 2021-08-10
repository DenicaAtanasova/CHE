namespace CHE.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IImagesService
    {
        Task UpdateAsync(Stream imageFile, string portfolioId);

        Task<string> CreateAvatarAsync(string portfolioId);

        Task DeleteAsync(string userId);
    }
}