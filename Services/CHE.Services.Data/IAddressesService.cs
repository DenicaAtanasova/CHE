namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<IEnumerable<TEntity>> GetAllCitiesAsync<TEntity>();

        Task<IEnumerable<TEntity>> GetAllNeighbourhoodsAsync<TEntity>();
    }
}