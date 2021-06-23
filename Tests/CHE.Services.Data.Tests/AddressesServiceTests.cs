
namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Web.InputModels.Cooperatives;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class AddressesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IAddressesService _addressesService;

        private readonly IEnumerable<Address> _addressesList = new List<Address>
            {
                new Address{City = "Varna", Neighbourhood = "Levski"},
                new Address{City = "Varna", Neighbourhood = "Asparuhovo"},
                new Address{City = "Razgrad", Neighbourhood = "H2O"},
                new Address{City = "Razgrad", Neighbourhood = "Levski"},
                new Address{City = "Sofia", Neighbourhood = "Levski"},
            };

        public AddressesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._addressesService = new AddressesService(this._dbContext);
        }

        [Fact]
        public async Task GetAddressIdAsync_ShouldWorkCorrectlyWithExistingAddress()
        {
            var testCity = "Varna";
            var testNeighbourhood = "Levski";

            var testedAddress = this._dbContext.Addresses.Add(
                new Address
                {
                    City = testCity,
                    Neighbourhood = testNeighbourhood
                }).Entity;

            this._dbContext.Addresses.Add(
                new Address
                {
                    City = "Varna",
                    Neighbourhood = "Asparuhovo"
                });

            this._dbContext.Addresses.Add(
                new Address
                {
                    City = "Sofia",
                    Neighbourhood = "Levski"
                });

            await this._dbContext.SaveChangesAsync();

            var addressId = await this._addressesService.GetAddressIdAsync(
                new CooperativeAddressInputModel
                {
                    City = testCity,
                    Neighbourhood = testNeighbourhood
                });

            Assert.Equal(testedAddress.Id, addressId);
        }

        [Fact]
        public async Task GetAddressIdAsync_ShouldWorkCorrectlyWithoutExistingAddress()
        {
            var testCity = "Varna";
            var testNeighbourhood = "Levski";

            this._dbContext.Addresses.Add(
                new Address
                {
                    City = "Varna",
                    Neighbourhood = "Asparuhovo"
                });

            this._dbContext.Addresses.Add(
                new Address
                {
                    City = "Sofia",
                    Neighbourhood = "Levski"
                });

            await this._dbContext.SaveChangesAsync();

            var addressId = await this._addressesService.GetAddressIdAsync(
                new CooperativeAddressInputModel
                {
                    City = testCity,
                    Neighbourhood = testNeighbourhood
                });

            var expectedAddress = await _dbContext.Addresses
                .SingleOrDefaultAsync(x => x.City == testCity && x.Neighbourhood == testNeighbourhood);

            Assert.Equal(expectedAddress.Id, addressId);
        }

        [Fact]
        public async Task GetAllCitiesAsync_ShouldWorkCorrectly()
        {
            this._dbContext.Addresses.AddRange(_addressesList);
            await this._dbContext.SaveChangesAsync();

            var cities = await this._addressesService.GetAllCitiesAsync();
            var expectedCitiesList = _addressesList
                .Select(x => x.City)
                .Distinct();

            Assert.Equal(expectedCitiesList, cities);
        }

        [Fact]
        public async Task GetAllNeighbourhoodsAsync_ShouldWorkCorrectly()
        {
            this._dbContext.Addresses.AddRange(_addressesList);
            await this._dbContext.SaveChangesAsync();

            var neighbourhoods = await this._addressesService.GetAllNeighbourhoodsAsync();
            var expectedNeighbourhoodsList = _addressesList
                .Select(x => x.Neighbourhood)
                .Distinct();

            Assert.Equal(expectedNeighbourhoodsList, neighbourhoods);
        }
    }
}