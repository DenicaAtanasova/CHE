namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<IEnumerable<TEntity>> GetAllCitiesAsync<TEntity>();

        Task<IEnumerable<TEntity>> GetAllNeighbourhoodsAsync<TEntity>();
    }

    public class AddressesService : IAddressesService
    {
        private readonly CheDbContext _dbContext;

        public AddressesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //TODO: Create class City
        public async Task<IEnumerable<TEntity>> GetAllCitiesAsync<TEntity>()
        {
            var cities = await this._dbContext.Addresses
                .To<TEntity>()
                .Distinct()
                .ToListAsync();

            return cities;
        }

        //TODO: Create class Neighbourhood
        public async Task<IEnumerable<TEntity>> GetAllNeighbourhoodsAsync<TEntity>()
        {
            var cities = await this._dbContext.Addresses
                .Where(x => x.Neighbourhood != null)
                .Distinct()
                .To<TEntity>()
                .ToListAsync();

            return cities;
        }
    }
}