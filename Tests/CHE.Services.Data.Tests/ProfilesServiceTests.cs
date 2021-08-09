namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Tests.Mocks;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Profiles;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    using static Mocks.MockConstants;

    public class ProfilesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IProfilesService _profilesService;

        public ProfilesServiceTests()
        {
            this._dbContext = DatabaseMock.Instance;

            this._profilesService = new ProfilesService(
                this._dbContext, 
                ImagesServiceMock.Instanse,
                AddressesServiceMock.Instance);

            AutoMapperConfig.RegisterMappings(
                typeof(ProfileInputModel).Assembly);
        }

        [Fact]
        public async Task GetByUserIdAsyncShouldReturnCorrectProfile()
        {
            var user = new CheUser();
            var profile = new Profile
            {
                Owner = new Teacher
                {
                    User = user
                }
            };

            this._dbContext.Profiles.Add(profile);
            await this._dbContext.SaveChangesAsync();

            var profileFromDb = await this._profilesService
                .GetByUserIdAsync<ProfileInputModel>(user.Id);

            Assert.NotNull(profileFromDb);
        }

        [Fact]
        public async Task GetByUserIdAsyncWithIncorrectId_ShouldReturnNull()
        {
            var user = new CheUser();
            var profile = new Profile
            {
                Owner = new Teacher
                {
                    User = user
                }
            };

            this._dbContext.Profiles.Add(profile);
            await this._dbContext.SaveChangesAsync();

            var profileFromDb = await this._profilesService
                .GetByUserIdAsync<ProfileInputModel>(Guid.NewGuid().ToString());

            Assert.Null(profileFromDb);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewProfile()
        {
            var userId = Guid.NewGuid().ToString();
            var profileId = await this._profilesService.CreateAsync(userId);

            var profileFromDb = await this._dbContext.Profiles
                .SingleOrDefaultAsync(x => x.OwnerId == userId);

            Assert.Equal(profileId, profileFromDb.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProfile()
        {
            var profile = new Profile
            {
                CreatedOn = DateTime.UtcNow,
                Education = "Education",
                Experience = "Experience",
                FirstName = "Maria",
                LastName = "Moneva",
                Interests = "Interests",
                OwnerId = Guid.NewGuid().ToString(),
                Skills = "Skills",
                SchoolLevel = SchoolLevel.Primary
            };

            this._dbContext.Profiles.Add(profile);
            await this._dbContext.SaveChangesAsync();

            var firstName = "Mariana";
            var lastName = "Coneva";
            var education = "Updated Education";
            var experience = "Updated Experience";
            var skills = "Updated Skills";
            var interests = "Updated Interests";
            var schoolLevel = "Secondary";
            var imageFile = new Mock<IFormFile>().Object;

            await this._profilesService.UpdateAsync(
                profile.Id,
                firstName,
                lastName,
                education,
                experience,
                skills,
                interests,
                schoolLevel,
                "Sofia",
                "Vitosha",
                null);

            var profileFromDb = await this._dbContext.Profiles
                .SingleOrDefaultAsync(x => x.Id == profile.Id);

            Assert.Equal(firstName, profileFromDb.FirstName);
            Assert.Equal(lastName, profileFromDb.LastName);
            Assert.Equal(education, profileFromDb.Education);
            Assert.Equal(experience, profileFromDb.Experience);
            Assert.Equal(skills, profileFromDb.Skills);
            Assert.Equal(interests, profileFromDb.Interests);
            Assert.Equal(schoolLevel, profileFromDb.SchoolLevel.ToString());
            Assert.Equal(AddressMock.Id, profileFromDb.AddressId);
        }
    }
}