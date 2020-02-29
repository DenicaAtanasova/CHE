namespace CHE.Services.Data
{

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeachersService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();

        Task<TEntity> GetByIdAsync<TEntity>(string id);
    }
}