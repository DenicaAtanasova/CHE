namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Enums;
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
        private const string FirstGrade = "First";
        private readonly string FirstGradeId = Guid.NewGuid().ToString();

        private readonly Address  Address = 
            new Address
            {
                City = "Sofia",
                Neighbourhood = "Dianabad"
            };

        private readonly string AddressId = Guid.NewGuid().ToString();

        private readonly CheDbContext _dbContext;
        private readonly ICooperativesService _cooperativesService;

        public CooperativesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            var gradesService = new Mock<IGradesService>();
            gradesService.Setup(x => x.GetGardeIdAsync(FirstGrade))
                .ReturnsAsync(FirstGradeId);

            var addressesService = new Mock<IAddressesService>();
            addressesService.Setup(x => x.GetAddressIdAsync(Address.City, Address.Neighbourhood))
                .ReturnsAsync(AddressId);

            this._cooperativesService = new CooperativesService(this._dbContext, gradesService.Object, addressesService.Object);

            AutoMapperConfig.RegisterMappings(
                typeof(CooperativeCreateInputModel).Assembly,
                typeof(CooperativeAllViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewCooperative()
        {
            var adminId = Guid.NewGuid().ToString();
            var name = "CoopName";
            var info = "CoopInfo";

            var cooperativeId = await this._cooperativesService.CreateAsync(
                adminId, name, info, FirstGrade, Address.City, Address.Neighbourhood);
            var cooperativeFromDb = await this._dbContext.Cooperatives.SingleOrDefaultAsync();
            var expectedCreatedOnDate = DateTime.UtcNow;

            Assert.Equal(cooperativeId, cooperativeFromDb.Id);
            Assert.Equal(adminId, cooperativeFromDb.AdminId);
            Assert.Equal(name, cooperativeFromDb.Name);
            Assert.Equal(info, cooperativeFromDb.Info);
            Assert.Equal(FirstGradeId, cooperativeFromDb.GradeId);
            Assert.Equal(AddressId, cooperativeFromDb.AddressId);
            Assert.NotNull(cooperativeFromDb.Schedule);
            Assert.Equal(expectedCreatedOnDate, 
                cooperativeFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCooperative()
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

            var name = "updatedName";
            var info = "updatedInfo";

            await this._cooperativesService
                .UpdateAsync(cooperative.Id, name, info, FirstGrade, Address.City, Address.Neighbourhood);
            var expectedModifiedOnDate = DateTime.UtcNow;

            var updatedCooperative = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperative.Id);

            Assert.Equal(name, updatedCooperative.Name);
            Assert.Equal(info, updatedCooperative.Info);
            Assert.Equal(FirstGradeId, updatedCooperative.GradeId);
            Assert.Equal(AddressId, updatedCooperative.AddressId);
            Assert.Equal(expectedModifiedOnDate, 
                updatedCooperative.ModifiedOn.Value, 
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_WithIncorrectId_ShouldDoNothing()
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

            var id = Guid.NewGuid().ToString();
            var name = "updatedName";
            var info = "updatedInfo";

            await this._cooperativesService
                .UpdateAsync(id, name, info, FirstGrade, Address.City, Address.Neighbourhood);

            var cooperativeFromDb = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperative.Id);

            Assert.Equal(cooperative.Name, cooperativeFromDb.Name);
            Assert.Equal(cooperative.Info, cooperativeFromDb.Info);
            Assert.Equal(cooperative.GradeId, cooperativeFromDb.GradeId);
            Assert.Equal(cooperative.AddressId, cooperativeFromDb.AddressId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCooperative()
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

            await this._cooperativesService.DeleteAsync(cooperative.Id);

            Assert.Empty(this._dbContext.Cooperatives);
        }

        [Fact]
        public async Task DeleteAsync_WithIncorrectId_ShouldDoNothing()
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

            await this._cooperativesService.DeleteAsync(Guid.NewGuid().ToString());

            Assert.NotEmpty(this._dbContext.Cooperatives);
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
        public async Task GetByIdAsync_WithIncorrectCooperativeId_ShouldReturnNull()
        {
            this._dbContext.Cooperatives.Add(
                new Cooperative
                {
                    Name = "CoopName",
                    Info = "CoopInfo"
                });
            await this._dbContext.SaveChangesAsync();

            Assert.Null(await this._cooperativesService
                .GetByIdAsync<CooperativeUpdateInputModel>(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetAllAsync_WithoutArgs_ShouldReturnCorrectCooperatives()
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
        [InlineData(6, 6, null, null, null)]
        [InlineData(0, 2, "First", null, null)]
        [InlineData(0, 6, null, "Sofia", null)]
        [InlineData(1, 6, null, null, "Levski")]
        [InlineData(0, 6, "First", "Sofia", null)]
        [InlineData(1, 6, "First", "Sofia", "Levski")]
        public async Task GetAllAsync_WithArgs_ShouldReturnCorrectCooperatives(
            int startIndex,
            int endIndex,
            string grade,
            string city,
            string neighbourhood)
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

            var filteredCooperatives = this.GetFilteredCollection(cooperativesList, grade, city, neighbourhood);
            var count = endIndex == 0
                ? filteredCooperatives.Count()
                : endIndex;

            var expectedCooperatives = filteredCooperatives
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            var cooperatives = await this._cooperativesService
                .GetAllAsync<CooperativeAllViewModel>(startIndex, endIndex, grade, city, neighbourhood);

            Assert.Equal(expectedCooperatives.Count, cooperatives.Count());

            var index = 0;
            foreach (var cooperative in cooperatives.OrderBy(x => x.Name))
            {
                Assert.Equal(expectedCooperatives[index++].Id, cooperative.Id);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 6)]
        [InlineData(6, 6)]
        public async Task GetAllByUserAsync_WhenUserIsAdmin_ShouldReturnCorrectCooperatives(int startIndex, int endIndex)
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
                .GetAllByUserAsync<CooperativeAllViewModel>(creatorId, CooperativeUserType.Admin, startIndex, endIndex);

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
            foreach (var cooperative in cooperatives.OrderBy(x => x.Name))
            {
                Assert.Equal(expectedCooperatives[index++].Id, cooperative.Id);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 6)]
        [InlineData(6, 6)]
        public async Task GetAllByUserAsync_WhenUserIsAdminOrMember_ShouldReturnCorrectCooperatives(int startIndex, int endIndex)
        {
            var userId = Guid.NewGuid().ToString();

            var cooperativesList = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    AdminId = userId
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    AdminId = userId
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    AdminId = Guid.NewGuid().ToString(),
                    Members = new List<CheUserCooperative>
                    {
                        new CheUserCooperative
                        {
                            CheUserId = userId
                        }
                    }
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
                .GetAllByUserAsync<CooperativeAllViewModel>(userId, CooperativeUserType.Admin | CooperativeUserType.Member, startIndex, endIndex);

            var count = endIndex == 0
                ? await this._dbContext.Cooperatives.CountAsync()
                : endIndex;

            var expectedCooperatives = cooperativesList
                .Where(x => x.AdminId == userId || x.Members.Any(x => x.CheUserId == userId))
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            Assert.Equal(expectedCooperatives.Count, cooperatives.Count());

            var index = 0;
            foreach (var cooperative in cooperatives.OrderBy(x => x.Name))
            {
                Assert.Equal(expectedCooperatives[index++].Id, cooperative.Id);
            }
        }

        [Fact]
        public async Task AddMemberAsync_ShouldAddMemberToCooperative()
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
        public async Task RemoveMemberAsync_ShouldRemoveFromCooperative()
        {
            var memberId = Guid.NewGuid().ToString();
            var cooperativeId = Guid.NewGuid().ToString();

            var members = new List<CheUserCooperative>
            {
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId,
                    CheUserId = memberId
                },
                new CheUserCooperative
                {
                    CooperativeId = Guid.NewGuid().ToString(),
                    CheUserId = Guid.NewGuid().ToString()
                },
            };

            this._dbContext.UserCooperatives.AddRange(members);

            await this._dbContext.SaveChangesAsync();

            await this._cooperativesService.RemoveMemberAsync(memberId, cooperativeId);

            var memberFromDb = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CooperativeId == cooperativeId && x.CheUserId == memberId);

            Assert.NotEmpty(this._dbContext.UserCooperatives);
            Assert.Null(memberFromDb);
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
            Assert.False(await this._cooperativesService
                .CheckIfMemberAsync(searchedMemberId, Guid.NewGuid().ToString()));
            Assert.False(await this._cooperativesService
                .CheckIfMemberAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task CheckIfAdminAsync_ShouldWorkCorrectly()
        {
            var adminId = Guid.NewGuid().ToString();
            var cooperative = new Cooperative
            {
                Name = "CoopName",
                Info = "CoopInfo",
                AdminId = adminId
            };

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            Assert.True(await this._cooperativesService
                .CheckIfAdminAsync(adminId, cooperative.Id));
            Assert.False(await this._cooperativesService
                .CheckIfAdminAsync(Guid.NewGuid().ToString(), cooperative.Id));
            Assert.False(await this._cooperativesService
                .CheckIfAdminAsync(adminId, Guid.NewGuid().ToString()));
            Assert.False(await this._cooperativesService
                .CheckIfAdminAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task CountAsync_WithoutArgs_ShouldReturnCorrectCount()
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
        public async Task CountAsync_WithArgs_ShouldReturnCorrectCount(
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


            var expectedCount = this.GetFilteredCollection(cooperatives, gradeFilter, cityFilter, neighbourhoodFilter).Count();
            var count = await this._cooperativesService.CountAsync(gradeFilter, cityFilter, neighbourhoodFilter);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public async Task CountByUserAsync_WithUserId_ShouldWorkCorrectly()
        {
            var userId = Guid.NewGuid().ToString();

            var cooperatives = new List<Cooperative>
            {
                new Cooperative
                {
                    Name = "Name1",
                    Info = "Info1",
                    AdminId = userId
                },
                new Cooperative
                {
                    Name = "Name2",
                    Info = "Info2",
                    AdminId = userId
                },
                new Cooperative
                {
                    Name = "Name3",
                    Info = "Info3",
                    AdminId = Guid.NewGuid().ToString(),
                    Members = new List<CheUserCooperative>
                    {
                        new CheUserCooperative
                        {
                            CheUserId = userId
                        }
                    }
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

            this._dbContext.Cooperatives.AddRange(cooperatives);
            await this._dbContext.SaveChangesAsync();

            var count = await this._cooperativesService.CountByUserAsync(userId);
            var expectedCount = cooperatives
                .Where(x => x.AdminId == userId || x.Members.Any(x => x.CheUserId == userId))
                .Count();

            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public async Task ChangeAdminAsync_ShouldWorkCorrectly()
        {
            var currentAdmin = new CheUser
            {
                UserName = "Admin"
            };

            var futureAdmin = new CheUser
            {
                UserName = "NewAdmin"
            };

            var cooperative = new Cooperative
            {
                Admin = currentAdmin,
                Members = new List<CheUserCooperative>
                {
                    new CheUserCooperative
                    {
                        CheUser = futureAdmin
                    }
                }
            };

            this._dbContext.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            await this._cooperativesService.ChangeAdminAsync(futureAdmin.Id, cooperative.Id);
            var cooperativeFromDb = await this._dbContext.Cooperatives.SingleOrDefaultAsync();

            Assert.Equal(futureAdmin.Id, cooperativeFromDb.AdminId);
        }

        [Fact]
        public async Task ChangeAdminAsync_ShouldTurnCurrentAdminToMember()
        {
            var currentAdmin = new CheUser
            {
                UserName = "Admin"
            };

            var futureAdmin = new CheUser
            {
                UserName = "NewAdmin"
            };

            var cooperative = new Cooperative
            {
                Admin = currentAdmin,
                Members = new List<CheUserCooperative>
                {
                    new CheUserCooperative
                    {
                        CheUser = futureAdmin
                    }
                }
            };

            this._dbContext.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            await this._cooperativesService.ChangeAdminAsync(futureAdmin.Id, cooperative.Id);
            var cooperativeFromDb = await this._dbContext.Cooperatives.SingleOrDefaultAsync();

            Assert.Contains(cooperativeFromDb.Members, x => x.CheUserId == currentAdmin.Id);
        }

        private IEnumerable<Cooperative> GetFilteredCollection(
            IEnumerable<Cooperative> cooperatives,
            string grade = null,
            string city = null,
            string neighbourhood = null)
        {
            if (grade != null)
            {
                cooperatives = cooperatives.Where(x => x.Grade.Value == grade).ToList();
            }

            if (city != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.City == city).ToList();
            }

            if (neighbourhood != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.Neighbourhood == neighbourhood).ToList();
            }

            return cooperatives;
        }
    }
}