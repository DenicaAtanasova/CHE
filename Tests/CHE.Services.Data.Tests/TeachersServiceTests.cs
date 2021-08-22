namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Data.Models.Enums;
    using CHE.Services.Data.Tests.Mocks;
    using CHE.Services.Mapping;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class TeachersServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly ITeachersService _teachersService;

        public TeachersServiceTests()
        {
            this._dbContext = DatabaseMock.Instance;

            this._teachersService = new TeachersService(this._dbContext, ProfilesServiceMock.Instance);

            AutoMapperConfig.RegisterMappings(
                typeof(TeacherDetailsViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var user = new CheUser();
            this._dbContext.Users.Add(user);
            await this._dbContext.SaveChangesAsync();

            var teacherId = await this._teachersService.CreateAsync(user.Id);

            var teacherFromDb = await this._dbContext.Teachers
                .SingleOrDefaultAsync(x => x.UserId == user.Id);

            var expectedCreatedOnDate = DateTime.UtcNow;

            Assert.Equal(teacherId, teacherFromDb.Id);
            Assert.Equal(expectedCreatedOnDate,
                teacherFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectTeacher()
        {
            var teacher = new Teacher();

            this._dbContext.Teachers.Add(teacher);
            await this._dbContext.SaveChangesAsync();

            var userFromDb = await this._teachersService
                .GetByIdAsync<TeacherDetailsViewModel>(teacher.Id);

            Assert.Equal(teacher.Id, userFromDb.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithIncorrectTeacherId_ShouldReturnNull()
        {
            var teacher = new Teacher();

            this._dbContext.Teachers.Add(teacher);
            await this._dbContext.SaveChangesAsync();

            Assert.Null(await this._teachersService
                .GetByIdAsync<TeacherDetailsViewModel>(Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task GetAllAsync_WithoutFilterArgs_ShouldReturnCorrectTeachers()
        {
            var teachersList = await this.SeedAndGetTeachers();

            var teachers = await this._teachersService
                .GetAllAsync<TeacherAllViewModel>();

            Assert.Equal(teachersList.Count(), teachers.Count());

            var index = 0;
            foreach (var teacher in teachers)
            {
                Assert.Equal(teachersList[index++].Id, teacher.Id);
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
        public async Task GetAllAsync_WithFilterArgs_ShouldReturnCorrectUsers(
            int startIndex,
            int endIndex,
            string schoolLevel,
            string city,
            string neighbourhood)
        {
            var teachersList = await this.SeedAndGetTeachers();

            var filteredTeachers = this.GetFilteredCollection(teachersList, schoolLevel, city, neighbourhood);

            var count = endIndex == 0
                ? filteredTeachers.Count()
                : endIndex;

            var expectedTeachers = filteredTeachers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .ToList();

            var teachersFromDb = await this._teachersService.GetAllAsync<TeacherAllViewModel>(
                startIndex,
                endIndex,
                schoolLevel,
                city,
                neighbourhood);

            Assert.Equal(expectedTeachers.Count, teachersFromDb.Count());

            var index = 0;
            foreach (var teacher in teachersFromDb)
            {
                Assert.Equal(expectedTeachers[index++].Id, teacher.Id);
            }
        }

        [Fact]
        public async Task CountAsync_WithoutFilterArgs_ShouldReturnCorrectCount()
        {
            var teachersList = await this.SeedAndGetTeachers();

            var teachersCount = await this._teachersService.CountAsync();

            Assert.Equal(teachersList.Count(), teachersCount);
        }

        [Theory]
        [InlineData("Primary", null, null)]
        [InlineData(null, "Sofia", null)]
        [InlineData(null, null, "Levski")]
        [InlineData("Primary", "Sofia", null)]
        [InlineData("Primary", "Sofia", "Levski")]
        public async Task CountAsync_WithFilterArgs_ShouldReturnCorrectCount(
           string schoolLevelFilter,
           string cityFilter,
           string neighbourhoodFilter)
        {
            var teachersList = await this.SeedAndGetTeachers();

            var teachersCount = await this._teachersService
                .CountAsync(schoolLevelFilter, cityFilter, neighbourhoodFilter);

            var filteredTeachers = this.GetFilteredCollection(teachersList, schoolLevelFilter, cityFilter, neighbourhoodFilter);

            Assert.Equal(filteredTeachers.Count(), teachersCount);
        }

        private async Task<IList<Teacher>> SeedAndGetTeachers()
        {
            var teachersList = new List<Teacher>
            {
                new Teacher
                {
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Primary,
                        Address = new Address
                        {
                            City = "Varna",
                            Neighbourhood = "Levski"
                        }
                    },
                    ReviewsReceived = new List<Review>
                    {
                        new Review
                        {
                            Rating = 2
                        }
                    }
                },
                new Teacher
                {
                    Profile = new Profile
                    {
                        SchoolLevel = SchoolLevel.Secondary,
                        Address = new Address
                        {
                            City = "Sofia",
                            Neighbourhood = "Dianabad"
                        }
                    },
                    ReviewsReceived = new List<Review>
                    {
                        new Review
                        {
                            Rating = 2
                        },
                        new Review
                        {
                            Rating = 1
                        }
                    }
                },
                new Teacher
                {
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
                new Teacher
                {
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

            this._dbContext.Teachers.AddRange(teachersList);
            await this._dbContext.SaveChangesAsync();

            return teachersList
                .OrderByDescending(x => x.ReviewsReceived
                    .Sum(x => x.Rating) / (x.ReviewsReceived.Count == 0 ? 1 : x.ReviewsReceived.Count))
                .ToList();
        }

        private IList<Teacher> GetFilteredCollection(
            IEnumerable<Teacher> teachers,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                teachers = teachers.Where(x => x.Profile.SchoolLevel == schoolLevel);
            }

            if (cityFilter != null)
            {
                teachers = teachers.Where(x => x.Profile.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                teachers = teachers.Where(x => x.Profile.Address.Neighbourhood == neighbourhoodFilter);
            }

            return teachers.ToList();
        }
    }
}