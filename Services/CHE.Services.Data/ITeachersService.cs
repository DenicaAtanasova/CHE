namespace CHE.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITeachersService
    {
        IQueryable<TEntity> GetAll<TEntity>();

        Task<TEntity> GetByIdAsync<TEntity>(string id);
    }
}