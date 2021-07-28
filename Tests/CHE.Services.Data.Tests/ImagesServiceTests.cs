namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Storage;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Xunit;

    public class ImagesServiceTests
    {
        private const string AvatarImageCaption = "Teacher_Avatar.png";
        private const string AvatarImageUrl = @"https://chestorage.blob.core.windows.net/uploads/Teacher_Avatar.png";

        private readonly string spaceUrl = "space-pillow-url";
        private readonly Stream spaceFileContent = new MemoryStream();

        private readonly string profileId = Guid.NewGuid().ToString();

        private readonly CheDbContext _dbContext;
        private IImagesService _imagesService;

        public ImagesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this._dbContext = new CheDbContext(options);

            var cloudStorageService = new Mock<IFileStorage>();
            cloudStorageService.Setup(x => x.UploadAsync(profileId, spaceFileContent))
                .ReturnsAsync(spaceUrl);

            this._imagesService = new ImagesService(cloudStorageService.Object, this._dbContext);
        }

        [Fact]
        public async Task CreateAvatarAsync_ShouldCreateNewAvatarImage()
        {
            var imageId = await this._imagesService.CreateAvatarAsync(profileId);

            var imageFromDb = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.ProfileId == profileId);

            var expectedCreatedOnDate = DateTime.UtcNow;

            Assert.Equal(imageId, imageFromDb.Id);
            Assert.Equal(AvatarImageCaption, imageFromDb.Caption);
            Assert.Equal(AvatarImageUrl, imageFromDb.Url);
            Assert.Equal(expectedCreatedOnDate,
                imageFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateImage()
        {
            var image = new Image
            { 
                ProfileId = profileId
            };

            this._dbContext.Images.Add(image);
            await this._dbContext.SaveChangesAsync();

            var spaceImageMock = new Mock<IFormFile>();
            spaceImageMock.Setup(x => x.OpenReadStream())
                .Returns(spaceFileContent);

            await this._imagesService.UpdateAsync(spaceImageMock.Object, profileId);

            var imageFromDb = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.ProfileId == profileId);

            Assert.Equal(image.Url, spaceUrl);
        }
    }
}