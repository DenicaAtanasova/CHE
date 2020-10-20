namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITeachersService
    {
        Task<TEntity> GetByIdAsync<TEntity>(string id);

        IQueryable<TEntity> GetAll<TEntity>();
    }
}