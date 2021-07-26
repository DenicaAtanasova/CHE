namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Storage;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImagesService : IImagesService
    {
        private const string DefaultImageCaption = "Teacher_Avatar.png";
        private const string DefaultImageUrl = @"https://chestorage.blob.core.windows.net/uploads/Teacher_Avatar.png";

        private readonly IFileStorage _cloudStorageService;
        private readonly CheDbContext _dbContext;

        public ImagesService(
            IFileStorage cloudStorageService,
            CheDbContext dbContext)
        {
            this._cloudStorageService = cloudStorageService;
            this._dbContext = dbContext;
        }

        public async Task<string> CreateAvatarAsync(string profileId)
        {
            var image = new Image
            {
                Caption = DefaultImageCaption,
                Url = DefaultImageUrl,
                ProfileId = profileId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Images.Add(image);
            await this._dbContext.SaveChangesAsync();

            return image.Id;
        }

        public async Task UpdateAsync(IFormFile imageFile, string profileId)
        {
            var currentImage = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.ProfileId == profileId);

            if (currentImage == null)
            {
                return;
            }

            if (currentImage.Caption != DefaultImageCaption)
            {
                await this._cloudStorageService.DeleteAsync(currentImage.Caption);
            }

            currentImage.Url = await this._cloudStorageService
                .UploadAsync(profileId, imageFile.OpenReadStream());

            this._dbContext.Images.Update(currentImage);
            await this._dbContext.SaveChangesAsync();
        }
    }
}