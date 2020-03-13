namespace CHE.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    using CHE.Data.Models;

    public class CooperativesServiceTests : BaseTest
    {
        private readonly string ID = Guid.NewGuid().ToString();
        private const string NAME = "Test name";
        private const string INFO = "Test info";
        private const string GRADE = "First";
        private const string CITY = "Test city";
        private const string NEIGHBOURHOOD = "Test neighbourhood";
        private const string STREET = "Test street";

        private readonly Cooperative TEST_COOPERATIVE;
        private readonly Address TEST_ADDRESS;

        private readonly CooperativesService _cooperativesService;

        public CooperativesServiceTests()
            : base()
        {
            this._cooperativesService = new CooperativesService(
                this.DbContext,
                this.Mapper,
                new GradesService(this.DbContext));

            this.TEST_COOPERATIVE = new Cooperative
            {
                Name = NAME,
                Info = INFO,
                Grade = new Grade
                {
                    Value = GRADE,
                    NumValue = 1
                }
            };

            this.TEST_ADDRESS = new Address
            {
                City = CITY,
                Neighbourhood = NEIGHBOURHOOD,
                Street = STREET
            };
        }

        #region CreateAsync
        [Fact]
        public async Task CreateAsyncShouldCreateCooperative()
        {
            var createSuccessful = await this._cooperativesService
                .CreateAsync(NAME, INFO, GRADE, ID);

            Assert.True(createSuccessful);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperative()
        {

            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updateSuccessful = await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, TEST_ADDRESS);

            Assert.True(updateSuccessful);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeNameWhenNameChanged()
        {

            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updatedName = "Updated name";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, updatedName, INFO, GRADE, TEST_ADDRESS);

            Assert.Equal(updatedName, TEST_COOPERATIVE.Name);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeInfoWhenInfoChanged()
        {
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updatedInfo = "Updated info";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, updatedInfo, GRADE, TEST_ADDRESS);

            Assert.Equal(updatedInfo, TEST_COOPERATIVE.Info);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeGradeWhenGradeChanged()
        {

            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();
            var initialGradeId = TEST_COOPERATIVE.GradeId;

            var updatedGrade = "Second";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, updatedGrade, TEST_ADDRESS);
            var updatedGradeId = TEST_COOPERATIVE.GradeId;

            Assert.NotEqual(initialGradeId, updatedGradeId);
        }

        [Fact]
        public async Task UpdateAsyncShouldChangeModifiedOnDateToDateTimeUtcNow()
        {
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var address = new Address
            {
                City = CITY
            };

            await this._cooperativesService.UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, address);
            var expectedDate = DateTime.UtcNow;
            var actualDate = TEST_COOPERATIVE.ModifiedOn;

            Assert.True((expectedDate - actualDate.Value).TotalSeconds < 0.1);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddress()
        {

            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();
            var initialAddress = TEST_COOPERATIVE.Address;

            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, TEST_ADDRESS);

            Assert.NotEqual<Address>(initialAddress, TEST_COOPERATIVE.Address);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressCityWhenCityChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updatedCity = "Sofia";
            var address = new Address
            {
                City = updatedCity
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, address);

            Assert.Equal(updatedCity, TEST_COOPERATIVE.Address.City);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressNeighbourhoodWhenNeighbourhoodChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updatedNeighbourhood = "Mladost";
            var address = new Address
            {
                Neighbourhood = updatedNeighbourhood
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, address);

            Assert.Equal(updatedNeighbourhood, TEST_COOPERATIVE.Address.Neighbourhood);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressStreetWhenStreetChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();

            var updatedStreet = "Main street";
            var address = new Address
            {
                Street = updatedStreet
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, NAME, INFO, GRADE, address);

            Assert.Equal(updatedStreet, TEST_COOPERATIVE.Address.Street);
        }
        #endregion

        // DeleteAsync

        // GetByIdAsync

        // GetAllAsync

        // GetCreatorAllByUsernameAsync

        // GetJoinRequestsAsync

        // AddMemberAsync

        // RemoveMemberAsync

        // LeaveAsync
    }
}
