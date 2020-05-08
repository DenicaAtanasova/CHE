namespace CHE.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using CHE.Data.Models;

    public class CooperativesServiceTests : BaseServiceTest
    {
        private readonly string CREATOR_ID = Guid.NewGuid().ToString();
        private const string USERNAME = "Username";

        private const string COOPERATIVE_NAME = "Name";
        private const string COOPERATIVE_INFO = "Info";
        private const string COOPERATIVE_GRADE = "First";

        private readonly CheUser testCreator;
        private readonly Cooperative testCooperetaive;
        private readonly Address testAddress;

        private readonly ICooperativesService _cooperativesService;
        private readonly IGradesService _gradesService;

        public CooperativesServiceTests()
            : base()
        {
            this._gradesService = this.ServiceProvider.GetRequiredService<IGradesService>();
            this._cooperativesService = this.ServiceProvider.GetRequiredService<ICooperativesService>();

            this.testAddress = this.SetAddress();
            this.testCooperetaive = this.SetCooperative();
            this.testCreator = this.SetUser();

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

        [Fact]
        public async Task CreateAsyncShouldSetCreatedOnDateToDateTimeUtcNow()
        {
            await this._cooperativesService
                .CreateAsync(COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, CREATOR_ID);

            var expectedDate = DateTime.UtcNow;
            var actualDate = await this.DbContext.Cooperatives
                .Where(x => x.CreatorId == CREATOR_ID)
                .Select(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            Assert.Equal(expectedDate, actualDate, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 100));
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperative()
        {
            var updateSuccessful = await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, this.testAddress);

            Assert.True(updateSuccessful);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeNameWhenNameChanged()
        {
            var updatedName = "Updated name";
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, updatedName, COOPERATIVE_INFO, COOPERATIVE_GRADE, this.testAddress);

            Assert.Equal(updatedName, this.testCooperetaive.Name);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeInfoWhenInfoChanged()
        {
            var updatedInfo = "Updated info";
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, updatedInfo, COOPERATIVE_GRADE, this.testAddress);

            Assert.Equal(updatedInfo, this.testCooperetaive.Info);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeGradeWhenGradeChanged()
        {
            var initialGradeId = this.testCooperetaive.GradeId;

            var updatedGrade = "Second";
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, updatedGrade, this.testAddress);
            var updatedGradeId = this.testCooperetaive.GradeId;

            Assert.NotEqual(initialGradeId, updatedGradeId);
        }

        [Fact]
        public async Task UpdateAsyncShouldSetModifiedOnDateToDateTimeUtcNow()
        {
            var address = new Address
            {
                City = "Plovdiv"
            };

            await this._cooperativesService.UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);
            var expectedDate = DateTime.UtcNow;
            var actualDate = this.testCooperetaive.ModifiedOn.Value;

            Assert.Equal(expectedDate, actualDate, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 300));
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressWhenAddressChanged()
        {
            var initialAddress = this.testCooperetaive.Address;

            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, this.testAddress);

            Assert.NotEqual<Address>(initialAddress, this.testCooperetaive.Address);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressCityWhenCityChanged()
        {
            this.testCooperetaive.Address = this.testAddress;
            await this.DbContext.SaveChangesAsync();

            var updatedCity = "Sofia";
            var address = new Address
            {
                City = updatedCity
            };
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedCity, this.testCooperetaive.Address.City);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressNeighbourhoodWhenNeighbourhoodChanged()
        {
            this.testCooperetaive.Address = this.testAddress;
            await this.DbContext.SaveChangesAsync();

            var updatedNeighbourhood = "Mladost";
            var address = new Address
            {
                Neighbourhood = updatedNeighbourhood
            };
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedNeighbourhood, this.testCooperetaive.Address.Neighbourhood);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateCooperativeAddressStreetWhenStreetChanged()
        {
            this.testCooperetaive.Address = this.testAddress;
            await this.DbContext.SaveChangesAsync();

            var updatedStreet = "Main street";
            var address = new Address
            {
                Street = updatedStreet
            };
            await this._cooperativesService
                .UpdateAsync(this.testCooperetaive.Id, COOPERATIVE_NAME, COOPERATIVE_INFO, COOPERATIVE_GRADE, address);

            Assert.Equal(updatedStreet, this.testCooperetaive.Address.Street);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsyncShouldDeleteCooperative()
        {
            var deleteSuccessful = await this._cooperativesService.DeleteAsync(this.testCooperetaive.Id);

            Assert.True(deleteSuccessful);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectCooperative()
        {
            var cooperative = await this._cooperativesService
                .GetByIdAsync<Cooperative>(this.testCooperetaive.Id);

            Assert.Equal(this.testCooperetaive.Id, cooperative.Id);
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GetAllAsyncShouldReturnAllCooperatives()
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
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE)               
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE)
                }
            };
            await this.DbContext.AddRangeAsync(cooperatives);
            await this.DbContext.SaveChangesAsync();

            var undeletedCoperatives = await this._cooperativesService
                .GetAllAsync<Cooperative>();

            var expectedCount = 4;
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
                    Creator = this.testCreator
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    Grade = await this._gradesService.GetByValueAsync(COOPERATIVE_GRADE),
                    Creator = this.testCreator
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
                .GetCreatorAllByUsernameAsync<Cooperative>(this.testCreator.UserName);

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
            await this._cooperativesService.AddMemberAsync(memberId, this.testCooperetaive.Id);

            var expectedMembersCount = 1;
            var actualMemebersCount = this.testCooperetaive.Members.Count;

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
                CooperativeId = this.testCooperetaive.Id
            };
            members.Add(firstMember);

            var secondMemberId = Guid.NewGuid().ToString();
            var secondMember = new CheUserCooperative
            {
                CheUserId = secondMemberId,
                CooperativeId = this.testCooperetaive.Id
            };
            members.Add(secondMember);

            await this.DbContext.UserCooperatives.AddRangeAsync(members);
            await this.DbContext.SaveChangesAsync();

            await this._cooperativesService.RemoveMemberAsync(firstMmemberId, this.testCooperetaive.Id);

            var expectedMembersCount = 1;
            var actualMemebersCount = this.testCooperetaive.Members.Count;

            Assert.Equal(expectedMembersCount, actualMemebersCount);
        }
        #endregion

        private async Task AddTestCooperativeAsync()
        {
            await this.DbContext.Cooperatives.AddAsync(this.testCooperetaive);
            await this.DbContext.SaveChangesAsync();
        }

        private Address SetAddress()
        {
            var address = new Address
            {
                City = "Test city",
                Neighbourhood = "Test neighbourhood",
                Street = "Test street"
            };

            return address;
        }

        private Cooperative SetCooperative()
        {
            var cooperative = new Cooperative
            {
                Name = "Test name",
                Info = "Test info",
                Grade = this._gradesService.GetByValueAsync(COOPERATIVE_GRADE).GetAwaiter().GetResult()               
            };

            return cooperative;
        }

        private CheUser SetUser()
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