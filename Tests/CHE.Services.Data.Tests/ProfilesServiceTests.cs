namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Profiles;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class ProfilesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IProfilesService _profilesService;
        private readonly Address address = new Address
        {
            City = "Varna",
            Neighbourhood = "Levski"
        };
        private readonly string addressId;

        public ProfilesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._dbContext.Addresses.Add(address);
            this._dbContext.SaveChanges();

            this.addressId = address.Id;

            var imagesService = new Mock<IImagesService>();

            var addresesService = new Mock<IAddressesService>();
            addresesService.Setup(x => x.GetAddressIdAsync(address.City, address.Neighbourhood))
            .ReturnsAsync(addressId);

            this._profilesService = new ProfilesService(this._dbContext, imagesService.Object, addresesService.Object);

            AutoMapperConfig.RegisterMappings(
                typeof(ProfileInputModel).Assembly);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnCorrectProfile()
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
        public async Task GetByUserIdAsync_WithIncorrectId_ShouldReturnNull()
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

            var profileFromDb = await this._dbContext.Profiles.SingleOrDefaultAsync(x => x.OwnerId == userId);

            Assert.Equal(profileId, profileFromDb.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProfile()
        {
            var profile = new Profile
            {
                Address = new Address
                {
                    City = "Sofia",
                    Neighbourhood = "Vitosha"
                },
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
                address.City,
                address.Neighbourhood,
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
            Assert.Equal(address.City, profileFromDb.Address.City);
            Assert.Equal(address.Neighbourhood, profileFromDb.Address.Neighbourhood);
        }
    }
}