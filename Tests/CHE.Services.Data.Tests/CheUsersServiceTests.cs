namespace CHE.Services.Data.Tests
{
    using CHE.Common;
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class CheUsersServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly ICheUsersService _cheUsersService;

        private string teacherRoleId;
        private string parentRoleId;

        public CheUsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            var cooperativesService = new Mock<ICooperativesService>();
            cooperativesService
                .Setup(x => x.AddMemberAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(async (string userId, string cooperativeId) => 
                {
                    this._dbContext.UserCooperatives.Add(
                        new CheUserCooperative 
                        { 
                            CheUserId = userId,
                            CooperativeId = cooperativeId
                        });
                    await this._dbContext.SaveChangesAsync();
                });

            var joinRequestsService = new Mock<IJoinRequestsService>();
            joinRequestsService
                .Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var reviewsService = new Mock<IReviewsService>();
            this._cheUsersService = new CheUsersService(
                this._dbContext,
                cooperativesService.Object,
                joinRequestsService.Object,
                reviewsService.Object);

            AutoMapperConfig.RegisterMappings(
                typeof(TeacherDetailsViewModel).Assembly);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectUser()
        {
            var user = new CheUser
            {
                UserName = "Maria",
            };

            this._dbContext.Users.Add(user);
            await this._dbContext.SaveChangesAsync();

            var userFromDb = await this._cheUsersService.GetByIdAsync<TeacherDetailsViewModel>(user.Id);

            Assert.Equal(user.Id, userFromDb.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithIncorrectUserId_ShouldReturnNull()
        {
            var user = new CheUser
            {
                UserName = "Maria",
            };

            this._dbContext.Users.Add(user);
            await this._dbContext.SaveChangesAsync();

            Assert.Null(await this._cheUsersService
                .GetByIdAsync<TeacherDetailsViewModel>(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetAllAsync_WithTeacherRoleAndWithoutFilterArgs_ShouldReturnCorrectUsers()
        {
            await this.AddRolesAsync();

            var teachers = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Teacher1"
                },
                new CheUser
                {
                    UserName = "Teacher2"
                },
                new CheUser
                {
                    UserName = "Teacher3"
                },
                new CheUser
                {
                    UserName = "Teacher4"
                },
            };
            this._dbContext.Users.AddRange(teachers);

            var parents = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Parent1"
                },
                new CheUser
                {
                    UserName = "Parent2"
                },
                new CheUser
                {
                    UserName = "Parent3"
                }
            };
            this._dbContext.AddRange(parents);

            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var teacher in teachers)
            {
                userRoles.Add(new IdentityUserRole<string>
                { 
                    UserId = teacher.Id,
                    RoleId = teacherRoleId
                });
            }

            foreach (var parent in parents)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = parent.Id,
                    RoleId = parentRoleId
                });
            }

            this._dbContext.UserRoles.AddRange(userRoles);
            await this._dbContext.SaveChangesAsync();

            var teachersFromDb = await this._cheUsersService
                .GetAllAsync<TeacherAllViewModel>(GlobalConstants.TeacherRole);

            Assert.Equal(teachers.Count, teachersFromDb.Count());

            var index = 0;
            foreach (var teacher in teachersFromDb.OrderBy(x => x.UserName))
            {
                Assert.Equal(teachers[index++].Id, teacher.Id);
            }
        }

        [Theory]
        [InlineData(0, 0, null, null, null)]
        [InlineData(-2, 0, null, null, null)]
        [InlineData(0, -1, null, null, null)]
        [InlineData(1, 6, null, null, null)]
        [InlineData(6, 6, null, null, null)]
        [InlineData(0, 2, "Primary", null, null)]
        [InlineData(0, 6, null, "Sofia", null)]
        [InlineData(1, 6, null, null, "Levski")]
        [InlineData(0, 6, "Primary", "Sofia", null)]
        [InlineData(1, 6, "Primary", "Sofia", "Levski")]
        public async Task GetAllAsync_WithTeacherRoleAndFilterArgs_ShouldReturnCorrectUsers(
            int startIndex,
            int endIndex,
            string schoolLevel,
            string city,
            string neighbourhood)
        {
            await this .AddRolesAsync();

            var teachers = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Teacher1",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Varna",
                            Neighbourhood = "Levski"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher2",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Secondary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher3",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                        Neighbourhood = "Vitosha"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher4",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
            };
            this._dbContext.Users.AddRange(teachers);

            var parents = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Parent1"
                },
                new CheUser
                {
                    UserName = "Parent2"
                },
                new CheUser
                {
                    UserName = "Parent3"
                }
            };
            this._dbContext.AddRange(parents);

            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var teacher in teachers)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = teacher.Id,
                    RoleId = teacherRoleId
                });
            }

            foreach (var parent in parents)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = parent.Id,
                    RoleId = parentRoleId
                });
            }

            this._dbContext.UserRoles.AddRange(userRoles);
            await this._dbContext.SaveChangesAsync();

            var filteredTeachers = this.GetFilteredCollection(teachers, schoolLevel, city, neighbourhood);

            var count = endIndex == 0
                ? filteredTeachers.Count()
                : endIndex;

            var expectedTeachers = filteredTeachers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            var teachersFromDb = await this._cheUsersService.GetAllAsync<TeacherAllViewModel>(
                GlobalConstants.TeacherRole,
                startIndex,
                endIndex,
                schoolLevel,
                city,
                neighbourhood);

            Assert.Equal(expectedTeachers.Count, teachersFromDb.Count());

            var index = 0;
            foreach (var teacher in teachersFromDb.OrderBy(x => x.UserName))
            {
                Assert.Equal(expectedTeachers[index++].Id, teacher.Id);
            }
        }

        [Fact]
        public async Task CountAsync_WithTeacherRoleAndWithoutFilterArgs_ShouldReturnCorrectCount()
        {
            await this .AddRolesAsync();

            var teachers = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Teacher1",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Varna",
                            Neighbourhood = "Levski"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher2",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Secondary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher3",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                        Neighbourhood = "Vitosha"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher4",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
            };
            this._dbContext.Users.AddRange(teachers);

            var parents = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Parent1"
                },
                new CheUser
                {
                    UserName = "Parent2"
                },
                new CheUser
                {
                    UserName = "Parent3"
                }
            };
            this._dbContext.AddRange(parents);

            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var teacher in teachers)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = teacher.Id,
                    RoleId = teacherRoleId
                });
            }

            foreach (var parent in parents)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = parent.Id,
                    RoleId = parentRoleId
                });
            }

            this._dbContext.UserRoles.AddRange(userRoles);
            await this._dbContext.SaveChangesAsync();

            var teachersCount = await this._cheUsersService.CountAsync(GlobalConstants.TeacherRole);

            Assert.Equal(teachers.Count, teachersCount);
        }

        [Theory]
        [InlineData("Primary", null, null)]
        [InlineData(null, "Sofia", null)]
        [InlineData(null, null, "Levski")]
        [InlineData("Primary", "Sofia", null)]
        [InlineData("Primary", "Sofia", "Levski")]
        public async Task CountAsync_WithTeacherRoleAndFilterArgs_ShouldReturnCorrectCount(
            string schoolLevelFilter,
            string cityFilter,
            string neighbourhoodFilter)
        {
            await this.AddRolesAsync();

            var teachers = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Teacher1",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Varna",
                            Neighbourhood = "Levski"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher2",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Secondary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher3",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                        Neighbourhood = "Vitosha"
                        }
                    }
                },
                new CheUser
                {
                    UserName = "Teacher4",
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    }
                },
            };
            this._dbContext.Users.AddRange(teachers);

            var parents = new List<CheUser>
            {
                new CheUser
                {
                    UserName = "Parent1"
                },
                new CheUser
                {
                    UserName = "Parent2"
                },
                new CheUser
                {
                    UserName = "Parent3"
                }
            };
            this._dbContext.AddRange(parents);

            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var teacher in teachers)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = teacher.Id,
                    RoleId = teacherRoleId
                });
            }

            foreach (var parent in parents)
            {
                userRoles.Add(new IdentityUserRole<string>
                {
                    UserId = parent.Id,
                    RoleId = parentRoleId
                });
            }

            this._dbContext.UserRoles.AddRange(userRoles);
            await this._dbContext.SaveChangesAsync();

            var teachersCount = await this._cheUsersService
                .CountAsync(GlobalConstants.TeacherRole, schoolLevelFilter, cityFilter, neighbourhoodFilter);
            var filteredTeachers = this.GetFilteredCollection(teachers, schoolLevelFilter, cityFilter, neighbourhoodFilter);

            Assert.Equal(filteredTeachers.Count(), teachersCount);
        }

        [Fact]
        public async Task AcceptRequestAsync_WithReceiverIdNull_ShouldAddSenderAsMember()
        {
            var senderId = Guid.NewGuid().ToString();
            var cooperativeId = Guid.NewGuid().ToString();

            await this._cheUsersService
                .AcceptRequestAsync(null, cooperativeId, senderId, null);

            var userCooperative = await this._dbContext.UserCooperatives
                .FirstOrDefaultAsync(x => x.CheUserId == senderId && x.CooperativeId == cooperativeId);

            Assert.NotNull(userCooperative);
        }

        [Fact]
        public async Task AcceptRequestAsync_WithReceiverId_ShouldAddReceiverAsMember()
        {
            var senderId = Guid.NewGuid().ToString();
            var receiverId = Guid.NewGuid().ToString();
            var cooperativeId = Guid.NewGuid().ToString();
            await this._cheUsersService
                .AcceptRequestAsync(null, cooperativeId, senderId, receiverId);

            var userCooperative = await this._dbContext.UserCooperatives
                .FirstOrDefaultAsync(x => x.CheUserId == receiverId && x.CooperativeId == cooperativeId);

            Assert.NotNull(userCooperative);
        }

        private async Task AddRolesAsync()
        {
            var teacherRole = new CheRole
            {
                Name = GlobalConstants.TeacherRole
            };
            _dbContext.Roles.Add(teacherRole);
            this.teacherRoleId = teacherRole.Id;

            var parentRole = new CheRole
            {
                Name = GlobalConstants.ParentRole
            };
            _dbContext.Roles.Add(parentRole);
            this.parentRoleId = parentRole.Id;

            await this._dbContext.SaveChangesAsync();
        }

        private IEnumerable<CheUser> GetFilteredCollection(
            IEnumerable<CheUser> users,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                return users.Where(x => x.Profile.SchoolLevel == schoolLevel);
            }

            if (cityFilter != null)
            {
                users = users.Where(x => x.Profile.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                users = users.Where(x => x.Profile.Address.Neighbourhood == neighbourhoodFilter);
            }

            return users;
        }
    }
}