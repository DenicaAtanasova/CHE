namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Tests.Mocks;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Xunit;

    using static Mocks.MockConstants;

    public class ImagesServiceTests
    {
        private const string AvatarImageCaption = "Teacher_Avatar.png";
        private const string AvatarImageUrl = @"https://chestorage.blob.core.windows.net/uploads/Teacher_Avatar.png";

        private readonly CheDbContext _dbContext;
        private readonly IImagesService _imagesService;

        public ImagesServiceTests()
        {
            this._dbContext = DatabaseMock.Instance;

            this._imagesService = new ImagesService(CloudStorageServiceMock.Instance, this._dbContext);
        }

        [Fact]
        public async Task CreateAvatarAsync_ShouldCreateNewAvatarImage()
        {
            var profileId = Guid.NewGuid().ToString();
            var imageId = await this._imagesService.CreateAvatarAsync(profileId);
            var expectedCreatedOnDate = DateTime.UtcNow;

            var imageFromDb = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.ProfileId == profileId);

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
            var profileId = Guid.NewGuid().ToString();

            var image = new Image
            { 
                ProfileId = profileId
            };

            this._dbContext.Images.Add(image);
            await this._dbContext.SaveChangesAsync();

            var spaceFileContent = new MemoryStream();

            await this._imagesService.UpdateAsync(spaceFileContent, profileId);

            var imageFromDb = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.ProfileId == profileId);

            Assert.Equal(CloudStorageMock.Url, imageFromDb.Url);
        }
    }
}