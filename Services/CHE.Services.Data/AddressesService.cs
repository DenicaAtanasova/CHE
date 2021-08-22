namespace CHE.Services.Data
{
    using CHE.Common.Extensions;
    using CHE.Data;
    using CHE.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddressesService : IAddressesService
    {
        private readonly CheDbContext _dbContext;

        public AddressesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<string> GetAddressIdAsync(string city, string neighbourhood)
        {
            var cityTitledCase = city.ToTitleCase();
            var neighbourhoodTitledCase = neighbourhood.ToTitleCase();

            var addressId = await this._dbContext.Addresses
                .AsNoTracking()
                .Where(x => x.City == cityTitledCase &&
                            x.Neighbourhood == neighbourhoodTitledCase)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            if (addressId is null)
            {
                return await this.CreateAsync(cityTitledCase, neighbourhoodTitledCase);
            }

            return addressId;
        }

        public async Task<IEnumerable<string>> GetAllCitiesAsync()
            => await this._dbContext.Addresses
                .Select(x => x.City)
                .Distinct()
                .ToListAsync();

        public async Task<IEnumerable<string>> GetAllNeighbourhoodsAsync()
            => await this._dbContext.Addresses
                .Where(x => x.Neighbourhood != null)
                .Select(x => x.Neighbourhood)
                .Distinct()
                .ToListAsync();

        private async Task<string> CreateAsync(string city, string neighbourhood)
        {
            var address = new Address
            {
                City = city,
                Neighbourhood = neighbourhood,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Addresses.Add(address);
            await this._dbContext.SaveChangesAsync();

            return address.Id;
        }
    }
}