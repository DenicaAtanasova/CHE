namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IPortfoliosService
    {
        Task<TEntity> GetByUserIdAsync<TEntity>(string username);

        Task<bool> UpdateAsync<TEntity>(string teacherId, TEntity portfolio, IFormFile imageFile);
    }
}