namespace CHE.Services.Data
{ 
    using Microsoft.AspNetCore.Http;

    using System.Threading.Tasks;

    public interface IProfilesService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);

        Task<string> CreateAsync(string userId);

        Task UpdateAsync(
            string userId,
            string firstName,
            string lastName,
            string education,
            string experience,
            string skills,
            string interests,
            string schoolLevel,
            string city,
            string neighbourhood,
            IFormFile imageFile);
    }
}