namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class CooperativesServiceTests
    {
        private const string FIRST_GRADE = "First";
        private readonly string FIRST_GRADE_ID = Guid.NewGuid().ToString();

        private readonly CooperativeAddressInputModel ADDRESS = new CooperativeAddressInputModel
        {
            City = "Sofia",
            Neighbourhood = "Dianabad"
        };
        private readonly string ADDRESS_ID = Guid.NewGuid().ToString();

        private readonly CheDbContext _dbContext;
        private readonly ICooperativesService _cooperativesService;

        public CooperativesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            var gradesService = new Mock<IGradesService>();
            gradesService.Setup(x => x.GetGardeIdAsync(FIRST_GRADE))
                .ReturnsAsync(FIRST_GRADE_ID);

            var addressesService = new Mock<IAddressesService>();
            addressesService.Setup(x => x.GetAddressIdAsync(ADDRESS))
                .ReturnsAsync(ADDRESS_ID);

            this._cooperativesService = new CooperativesService(this._dbContext, gradesService.Object, addressesService.Object);

            AutoMapperConfig.RegisterMappings(
                typeof(CooperativeCreateInputModel).Assembly,
                typeof(CooperativeAllViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var creatorId = Guid.NewGuid().ToString();

            var cooperative = new CooperativeCreateInputModel
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Grade = FIRST_GRADE,
                Address = ADDRESS
            };

            var cooperativeId = await this._cooperativesService.CreateAsync(creatorId, cooperative);
            var cooperativeFromDb = await this._dbContext.Cooperatives.SingleOrDefaultAsync();
            var expectedCreatedOnDate = DateTime.UtcNow;

            Assert.Equal(cooperativeId, cooperativeFromDb.Id);
            Assert.Equal(creatorId, cooperativeFromDb.AdminId);
            Assert.Equal(cooperative.Name, cooperativeFromDb.Name);
            Assert.Equal(cooperative.Info, cooperativeFromDb.Info);
            Assert.Equal(FIRST_GRADE_ID, cooperativeFromDb.GradeId);
            Assert.Equal(ADDRESS_ID, cooperativeFromDb.AddressId);
            Assert.NotNull(cooperativeFromDb.Schedule);
            Assert.Equal(expectedCreatedOnDate, 
                cooperativeFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var creatorId = Guid.NewGuid().ToString();

            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Grade = new Grade
                {
                    NumValue = 2,
                    Value = "Second"
                },
                Address = new Address
                {
                    City = "Varna",
                    Neighbourhood = "Levski"
                },
                AdminId = Guid.NewGuid().ToString()
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();
            this._dbContext.Entry(cooperative).State = EntityState.Detached;

            var cooperativeUpdateModel = new CooperativeUpdateInputModel
            {
                Id = cooperative.Id,
                Name = "updatedName",
                Info = "updatedInfo",
                Grade = FIRST_GRADE,
                Address = ADDRESS,
                CreatorId = cooperative.AdminId,
                CreatedOn = cooperative.CreatedOn
            };

            await this._cooperativesService
                .UpdateAsync(cooperativeUpdateModel);
            var expectedModifiedOnDate = DateTime.UtcNow;

            var updatedCooperative = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperative.Id);

            Assert.Equal(cooperativeUpdateModel.Name, updatedCooperative.Name);
            Assert.Equal(cooperativeUpdateModel.Info, updatedCooperative.Info);
            Assert.Equal(cooperativeUpdateModel.CreatorId, updatedCooperative.AdminId);
            Assert.Equal(FIRST_GRADE_ID, updatedCooperative.GradeId);
            Assert.Equal(ADDRESS_ID, updatedCooperative.AddressId);
            Assert.Equal(ADDRESS_ID, updatedCooperative.AddressId);
            Assert.Equal(expectedModifiedOnDate, 
                updatedCooperative.ModifiedOn.Value, 
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Grade = new Grade
                {
                    NumValue = 2,
                    Value = "Second"
                },
                Address = new Address
                {
                    City = "Varna",
                    Neighbourhood = "Levski"
                },
                AdminId = Guid.NewGuid().ToString()
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();
            this._dbContext.Entry(cooperative).State = EntityState.Detached;

            await this._cooperativesService.DeleteAsync(cooperative.Id);

            Assert.Empty(this._dbContext.Cooperatives);
            Assert.Empty(this._dbContext.Schedules);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCooperative()
        {
            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Grade = new Grade
                {
                    NumValue = 2,
                    Value = "Second"
                },
                Address = new Address
                {
                    City = "Varna",
                    Neighbourhood = "Levski"
                },
                AdminId = Guid.NewGuid().ToString()
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            var cooperativeFromDb = await this._cooperativesService
                .GetByIdAsync<CooperativeUpdateInputModel>(cooperative.Id);

            Assert.Equal(cooperative.Id, cooperativeFromDb.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWithIncorrectId()
        {
            this._dbContext.Cooperatives.Add(
                new Cooperative
                {
                    Name = "CoopName",
                    Info = "CoopInfo"
                });
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._cooperativesService
                .GetByIdAsync<CooperativeUpdateInputModel>(Guid.NewGuid().ToString());

            Assert.Null(actualRequest);
        }

        [Fact]
        public async Task GetAllAsync_ShouldWorkCorrectlyWithoutArgs()
        {
            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                }
            };
            await this._dbContext.AddRangeAsync(cooperatives);
            await this._dbContext.SaveChangesAsync();

            var cooperativesFromDb = await this._cooperativesService
                .GetAllAsync<CooperativeAllViewModel>();

            Assert.Equal(cooperatives.Count, cooperativesFromDb.Count());

            var index = 0;
            foreach (var cooperative in cooperativesFromDb)
            {
                Assert.Equal(cooperatives[index++].Id, cooperative.Id);
            }
        }

        [Theory]
        [InlineData(0, 0, null, null, null)]
        [InlineData(-2, 0, null, null, null)]
        [InlineData(0, -1, null, null, null)]
        [InlineData(1, 6, null, null, null)]
        [InlineData(0, 2, "First", null, null)]
        [InlineData(0, 6, null, "Sofia", null)]
        [InlineData(1, 6, null, null, "Levski")]
        [InlineData(0, 6, "First", "Sofia", null)]
        [InlineData(1, 6, "First", "Sofia", "Levski")]
        public async Task GetAllAsync_ShouldWorkCorrectlyWithArgs(
            int startIndex,
            int endIndex,
            string gradeFilter,
            string cityFilter,
            string neighbourhoodFilter)
        {
            var cooperativesList = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    Grade = new Grade
                    {
                        NumValue = 1,
                        Value = "First"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Dianabad"
                    }
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    Grade = new Grade
                    {
                        NumValue = 1,
                        Value = "First"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Levski"
                    }
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    Grade = new Grade
                    {
                        NumValue = 2,
                        Value = "Second"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Vitosha"
                    }
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    Grade = new Grade
                    {
                        NumValue = 3,
                        Value = "Third"
                    },
                    Address = new Address
                    {
                        City = "Varna",
                        Neighbourhood = "Levski"
                    }
                }
            };
            await this._dbContext.AddRangeAsync(cooperativesList);
            await this._dbContext.SaveChangesAsync();

            var cooperatives = await this._cooperativesService
                .GetAllAsync<CooperativeAllViewModel>(startIndex, endIndex, gradeFilter, cityFilter, neighbourhoodFilter);

            var count = endIndex == 0
                ? await this._dbContext.Cooperatives.CountAsync()
                : endIndex;

            var expectedCooperatives = this.GetFilterCollection(cooperativesList, gradeFilter, cityFilter, neighbourhoodFilter)
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            Assert.Equal(expectedCooperatives.Count, cooperatives.Count());

            var index = 0;
            foreach (var cooperative in cooperatives)
            {
                Assert.Equal(expectedCooperatives[index++].Id, cooperative.Id);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 6)]
        public async Task GetAllByCreatorAsync_ShouldWorkCorrectly(int startIndex, int endIndex)
        {
            var creatorId = Guid.NewGuid().ToString();

            var cooperativesList = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    AdminId = Guid.NewGuid().ToString()
                },
                new Cooperative
                {
                    Name = "Name5",
                    Info = "Info5",
                    AdminId = Guid.NewGuid().ToString()
                }
            };

            this._dbContext.Cooperatives.AddRange(cooperativesList);
            await this._dbContext.SaveChangesAsync();

            var cooperatives = await this._cooperativesService
                .GetAllByCreatorAsync<CooperativeAllViewModel>(creatorId, startIndex, endIndex);

            var count = endIndex == 0
                ? await this._dbContext.Cooperatives.CountAsync()
                : endIndex;

            var expectedCooperatives = cooperativesList
                .Where(x => x.AdminId == creatorId)
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            Assert.Equal(expectedCooperatives.Count, cooperatives.Count());

            var index = 0;
            foreach (var cooperative in cooperatives)
            {
                Assert.Equal(expectedCooperatives[index++].Id, cooperative.Id);
            }
        }

        //TODO: Test if member exists?
        [Fact]
        public async Task AddMemberAsync_ShouldWorkCorrectly()
        {
            var memberId = Guid.NewGuid().ToString();
            var cooperativeId = Guid.NewGuid().ToString();
            await this._cooperativesService.AddMemberAsync(memberId, cooperativeId);

            var memberFromDb = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CheUserId == memberId && x.CooperativeId == cooperativeId);

            var expectedMembersCount = 1;
            Assert.Equal(expectedMembersCount, this._dbContext.UserCooperatives.Count());
        }

        [Fact]
        public async Task RemoveMemberAsync_ShouldWorkCorrectly()
        {
            var memberId = Guid.NewGuid().ToString();
            var cooperativeId = Guid.NewGuid().ToString();

            var member = new CheUserCooperative
            {
                CooperativeId = cooperativeId,
                CheUserId = memberId
            };
            this._dbContext.UserCooperatives.Add(member);

            await this._dbContext.SaveChangesAsync();
            this._dbContext.Entry(member).State = EntityState.Detached;

            await this._cooperativesService.RemoveMemberAsync(memberId, cooperativeId);

            var expectedMembersCount = 0;

            Assert.Equal(expectedMembersCount, this._dbContext.UserCooperatives.Count());
        }

        [Fact]
        public async Task GetMembersAsync_ShouldWorkCorrectly()
        {
            var members = new List<CheUserCooperative>
            {
                new CheUserCooperative
                {
                    CheUser = new CheUser
                    {
                        UserName = "UserName1"
                    }
                },
                new CheUserCooperative
                {
                    CheUser = new CheUser
                    {
                        UserName = "UserName2"
                    }
                },
                new CheUserCooperative
                {
                    CheUser = new CheUser
                    {
                        UserName = "UserName3"
                    }
                },
            };

            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Members = members,
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            var memebersFromDb = await this._cooperativesService.GetMembersAsync<CooperativeUserDetailsViewModel>(cooperative.Id);

            Assert.Equal(members.Count, memebersFromDb.Count());

            var index = 0;
            foreach (var member in memebersFromDb)
            {
                Assert.Equal(members[index++].CheUserId, member.UserId);
            }
        }

        [Fact]
        public async Task CheckIfMemberAsync_ShouldWorkCorrectly()
        {
            var searchedMemberId = Guid.NewGuid().ToString();
            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                Members = new List<CheUserCooperative>
                { 
                    new CheUserCooperative
                    {
                        CheUserId = searchedMemberId
                    },
                    new CheUserCooperative
                    {
                        CheUserId = Guid.NewGuid().ToString()
                    }
                }
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            Assert.True(await this._cooperativesService
                .CheckIfMemberAsync(searchedMemberId, cooperative.Id));
            Assert.False(await this._cooperativesService
                .CheckIfMemberAsync(Guid.NewGuid().ToString(), cooperative.Id));
        }

        [Fact]
        public async Task CheckIfAdminAsync_ShouldWorkCorrectly()
        {
            var creatorId = Guid.NewGuid().ToString();
            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                AdminId = creatorId
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            Assert.True(await this._cooperativesService
                .CheckIfAdminAsync(creatorId, cooperative.Id));
            Assert.False(await this._cooperativesService
                .CheckIfAdminAsync(Guid.NewGuid().ToString(), cooperative.Id));
        }

        [Fact]
        public async Task CountAsync_ShouldWorkCorrectlyWithoutArgs()
        {
            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1"
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2"
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3"
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4"
                }
            };

            this._dbContext.Cooperatives.AddRange(cooperatives);
            await this._dbContext.SaveChangesAsync();

            var count = await this._cooperativesService.CountAsync();

            Assert.Equal(cooperatives.Count, count);
        }

        [Theory]
        [InlineData("First", null, null)]
        [InlineData(null, "Sofia", null)]
        [InlineData(null, null, "Levski")]
        [InlineData("First", "Sofia", null)]
        [InlineData("First", "Sofia", "Levski")]
        public async Task CountAsync_ShouldWorkCorrectlyWithArgs(
            string gradeFilter,
            string cityFilter,
            string neighbourhoodFilter)
        {
            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    Grade = new Grade
                    {
                        NumValue = 1,
                        Value = "First"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Dianabad"
                    }
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    Grade = new Grade
                    {
                        NumValue = 1,
                        Value = "First"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Levski"
                    }
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    Grade = new Grade
                    {
                        NumValue = 2,
                        Value = "Second"
                    },
                    Address = new Address
                    {
                        City = "Sofia",
                        Neighbourhood = "Vitosha"
                    }
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    Grade = new Grade
                    {
                        NumValue = 3,
                        Value = "Third"
                    },
                    Address = new Address
                    {
                        City = "Varna",
                        Neighbourhood = "Levski"
                    }
                }
            };
            await this._dbContext.AddRangeAsync(cooperatives);
            await this._dbContext.SaveChangesAsync();


            var expectedCount = this.GetFilterCollection(cooperatives, gradeFilter, cityFilter, neighbourhoodFilter).Count();
            var count = await this._cooperativesService.CountAsync(gradeFilter, cityFilter, neighbourhoodFilter);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public async Task CountAsync_ShouldWorkCorrectlyWithUserId()
        {
            var creatorId = Guid.NewGuid().ToString();

            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    AdminId = creatorId
                },
                new Cooperative
                {
                    Name = "Name4",
                    Info = "Info4",
                    AdminId = Guid.NewGuid().ToString()
                }
            };

            this._dbContext.Cooperatives.AddRange(cooperatives);
            await this._dbContext.SaveChangesAsync();

            var count = await this._cooperativesService.CountAsync(creatorId);
            var expectedCount = cooperatives.Where(x => x.AdminId == creatorId).Count();

            Assert.Equal(expectedCount, count);
        }

        private IEnumerable<Cooperative> GetFilterCollection(
            IEnumerable<Cooperative> cooperatives,
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            if (gradeFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Grade.Value == gradeFilter).ToList();
            }

            if (cityFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.City == cityFilter).ToList();
            }

            if (neighbourhoodFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.Neighbourhood == neighbourhoodFilter).ToList();
            }

            return cooperatives;
        }
    }
}