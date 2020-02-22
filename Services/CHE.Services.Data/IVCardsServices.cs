namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    public interface IVCardsServices
    {
        Task CreateAsync(string firstName, string lastName, string education, string experience, string skills, string interests, string owner, params string[] grades);

        Task DeleteAsync(string id);

        Task UpdateAsync<TEntity>(string id, TEntity entity);

        Task<TEntity> GetById<TEntity>(string id);
    }
}