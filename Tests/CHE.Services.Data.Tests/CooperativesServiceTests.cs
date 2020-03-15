namespace CHE.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Xunit;

    using CHE.Data.Models;

    public class CooperativesServiceTests : BaseTest
    {
        private readonly string CREATOR_ID;
        private const string USERNAME = "Test username";

        private const string COOPERATIVE_NAME = "Test name";
        private const string COOPERATIVE_INFO = "Test info";
        private const string COOPERATIVE_GRADE = "First";

        private const string CITY = "Test city";
        private const string NEIGHBOURHOOD = "Test neighbourhood";
        private const string STREET = "Test street";

        private readonly CheUser TEST_CREATOR;
        private readonly Cooperative TEST_COOPERATIVE;
        private readonly Address TEST_ADDRESS;

        private readonly CooperativesService _cooperativesService;
        private readonly GradesService _gradesService;

        public CooperativesServiceTests()
            : base()
        {
            this._gradesService = new GradesService(this.DbContext);
            this._cooperativesService = new CooperativesService(
                this.DbContext,
                this.Mapper,
                this._gradesService);

            CREATOR_ID = Guid.NewGuid().ToString();
            TEST_ADDRESS = this.CreateAddress();
            TEST_COOPERATIVE = this.CreateCooperative();
            TEST_CREATOR = this.CreateUser();

            this.AddFirstAndSecondGradesAsync().GetAwaiter().GetResult();
            this.AddTestCooperativeAsync().GetAwaiter().GetResult();
        }

        #region CreateAsync
        [Fact]
        public async Task CreateAsyncShouldCreateCooperative()
        {
            var createSuccessful = await this._cooperativesService
                .CreateAsync(COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, CREATOR_ID);
            Assert.True(createSuccessful);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperative()
        {
            var updateSuccessful = await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, TEST_ADDRESS);

            Assert.True(updateSuccessful);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeNameWhenNameChanged()
        {
            var updatedName = "Updated name";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, updatedName, COOPERATIVE_INFO, COOPERATIVE_GRADE, TEST_ADDRESS);

            Assert.Equal(updatedName, TEST_COOPERATIVE.Name);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeInfoWhenInfoChanged()
        {
            var updatedInfo = "Updated info";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, updatedInfo, COOPERATIVE_GRADE, TEST_ADDRESS);

            Assert.Equal(updatedInfo, TEST_COOPERATIVE.Info);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeGradeWhenGradeChanged()
        {
            var initialGradeId = TEST_COOPERATIVE.GradeId;

            var updatedGrade = "Second";
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, updatedGrade, TEST_ADDRESS);
            var updatedGradeId = TEST_COOPERATIVE.GradeId;

            Assert.NotEqual(initialGradeId, updatedGradeId);
        }

        [Fact]
        public async Task UpdateAsyncShouldSetModifiedOnDateToDateTimeUtcNow()
        {
            var address = new Address
            {
                City = CITY
            };

            await this._cooperativesService.UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);
            var expectedDate = DateTime.UtcNow;
            var actualDate = TEST_COOPERATIVE.ModifiedOn;

            Assert.True((expectedDate - actualDate.Value).TotalSeconds < 0.1);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddress()
        {
            var initialAddress = TEST_COOPERATIVE.Address;

            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, TEST_ADDRESS);

            Assert.NotEqual<Address>(initialAddress, TEST_COOPERATIVE.Address);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressCityWhenCityChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.SaveChangesAsync();

            var updatedCity = "Sofia";
            var address = new Address
            {
                City = updatedCity
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedCity, TEST_COOPERATIVE.Address.City);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressNeighbourhoodWhenNeighbourhoodChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.SaveChangesAsync();

            var updatedNeighbourhood = "Mladost";
            var address = new Address
            {
                Neighbourhood = updatedNeighbourhood
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedNeighbourhood, TEST_COOPERATIVE.Address.Neighbourhood);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressStreetWhenStreetChanged()
        {
            TEST_COOPERATIVE.Address = TEST_ADDRESS;
            await this.DbContext.SaveChangesAsync();

            var updatedStreet = "Main street";
            var address = new Address
            {
                Street = updatedStreet
            };
            await this._cooperativesService
                .UpdateAsync(TEST_COOPERATIVE.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedStreet, TEST_COOPERATIVE.Address.Street);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsyncShouldDeleteCooperative()
        {
            var id = TEST_COOPERATIVE.Id;

            var deleteSuccessful = await this._cooperativesService.DeleteAsync(id);

            Assert.True(deleteSuccessful);
        }

        [Fact]
        public async Task DeleteAsyncShouldChangeIsDeletedToTrue()
        {
            var id = TEST_COOPERATIVE.Id;

            var deleteSuccessful = await this._cooperativesService.DeleteAsync(id);

            Assert.True(TEST_COOPERATIVE.IsDeleted);
        }

        [Fact]
        public async Task DeleteAsyncShouldSetDeletedOnDateToDateTimeUtcNow()
        {
            var id = TEST_COOPERATIVE.Id;
            await this._cooperativesService.DeleteAsync(id);

            var expectedDate = DateTime.UtcNow;
            var actualDate = TEST_COOPERATIVE.DeletedOn;

            Assert.True((expectedDate - actualDate.Value).TotalSeconds < 0.1);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsyncShouldReturnCooperative()
        {
            var cooperative = await this._cooperativesService
                .GetByIdAsync<Cooperative>(TEST_COOPERATIVE.Id);

            Assert.Equal(TEST_COOPERATIVE.Id, cooperative.Id);
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GetAllAsyncShouldReturnAllUndeletedCooperatives()
        {
            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE)
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE),
                    IsDeleted = true                   
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE),
                    IsDeleted = true
                }
            };
            await this.DbContext.AddRangeAsync(cooperatives);
            await this.DbContext.SaveChangesAsync();

            var undeletedCoperatives = await this._cooperativesService
                .GetAllAsync<Cooperative>();

            var expectedCount = 2;
            var actualCount = undeletedCoperatives.Count();

            Assert.Equal(expectedCount, actualCount);
        }
        #endregion

        #region GetCreatorAllByUsernameAsync
        [Fact]
        public async Task GetCreatorAllByUsernameAsyncShouldReturnAllCreatorCooperatives()
        {
            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE),
                    Creator = TEST_CREATOR
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE),
                    Creator = TEST_CREATOR
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE)
                }
            };

            await this.DbContext.Cooperatives.AddRangeAsync(cooperatives);
            await this.DbContext.SaveChangesAsync();

            var creatorCooperatives = await this._cooperativesService
                .GetCreatorAllByUsernameAsync<Cooperative>(TEST_CREATOR.UserName);

            var expectedCooperativesCount = 2;
            var actualCooperativesCount = creatorCooperatives.Count();

            Assert.Equal(expectedCooperativesCount, actualCooperativesCount);
        }
        #endregion

        #region AddMemberAsync
        [Fact]
        public async Task AddMemberAsyncShouldAddMemeber()
        {
            var memberId = Guid.NewGuid().ToString();
            await this._cooperativesService.AddMemberAsync(memberId, TEST_COOPERATIVE.Id);

            var expectedMembersCount = 1;
            var actualMemebersCount = TEST_COOPERATIVE.Members.Count;

            Assert.Equal(expectedMembersCount, actualMemebersCount);
        }
        #endregion

        #region RemoveMemberAsync
        [Fact]
        public async Task RemoveMemberAsyncShouldRemoveMemeber()
        {
            var members = new List<CheUserCooperative>();

            var firstMmemberId = Guid.NewGuid().ToString();
            var firstMember = new CheUserCooperative
            {
                CheUserId = firstMmemberId,
                CooperativeId = TEST_COOPERATIVE.Id
            };
            members.Add(firstMember);

            var secondMemberId = Guid.NewGuid().ToString();
            var secondMember = new CheUserCooperative
            {
                CheUserId = secondMemberId,
                CooperativeId = TEST_COOPERATIVE.Id
            };
            members.Add(secondMember);

            await this.DbContext.UserCooperatives.AddRangeAsync(members);
            await this.DbContext.SaveChangesAsync();

            await this._cooperativesService.RemoveMemberAsync(firstMmemberId, TEST_COOPERATIVE.Id);

            var expectedMembersCount = 1;
            var actualMemebersCount = TEST_COOPERATIVE.Members.Count;

            Assert.Equal(expectedMembersCount, actualMemebersCount);
        }
        #endregion

        // LeaveAsync

        private async Task AddTestCooperativeAsync()
        {
            await this.DbContext.Cooperatives.AddAsync(TEST_COOPERATIVE);
            await this.DbContext.SaveChangesAsync();
        }

        private Address CreateAddress()
        {
            var address = new Address
            {
                City = CITY,
                Neighbourhood = NEIGHBOURHOOD,
                Street = STREET
            };

            return address;
        }

        private Cooperative CreateCooperative()
        {
            var cooperative = new Cooperative
            {
                Name = COOPERATIVE_NAME,
                Info = COOPERATIVE_INFO,
                Grade = this._gradesService.GetByValueAsync(COOPERATIVE_GRADE).GetAwaiter().GetResult()
            };

            return cooperative;
        }

        private CheUser CreateUser()
        {
            var user = new CheUser
            {
                Id = CREATOR_ID,
                UserName = USERNAME,
            };

            return user;
        }

        private async Task AddFirstAndSecondGradesAsync()
        {
            var grades = new List<Grade>
            {
                new Grade
                {
                    Value = "First",
                    NumValue = 1
                },
                new Grade
                {
                    Value = "Second",
                    NumValue = 2
                }
            };

            await this.DbContext.Grades.AddRangeAsync(grades);
            await this.DbContext.SaveChangesAsync();
        }
    }
}