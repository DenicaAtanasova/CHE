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
        private const string DEFAULT_IMAGE_CAPTION = "Teacher_Avatar.png";
        private const string DEFAULT_IMAGE_URL = @"https://chestorage.blob.core.windows.net/uploads/Teacher_Avatar.png";

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
                Caption = DEFAULT_IMAGE_CAPTION,
                Url = DEFAULT_IMAGE_URL,
                ProfileId = profileId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Images.Add(image);
            await this._dbContext.SaveChangesAsync();

            return image.Id;
        }

        public async Task<string> UpdateAsync(IFormFile imageFile, string profileId)
        {
            var currentImage = await this._dbContext.Images
                .Where(x => x.ProfileId == profileId)
                .Select(x => new
                {
                    Id = x.Id,
                    Caption = x.Caption
                })
                .FirstOrDefaultAsync();

            if (currentImage.Caption != DEFAULT_IMAGE_CAPTION)
            {
                await this._cloudStorageService.DeleteAsync(currentImage.Caption);
            }

            var url = await this._cloudStorageService
                .UploadAsync(imageFile.FileName, imageFile.OpenReadStream());

            this.DeleteImage(currentImage.Id);

            return await this.CreateImageAsync(imageFile.FileName, url, profileId);
        }

        private async Task<string> CreateImageAsync(string fileName, string url, string profileId)
        {
            var image = new Image
            {
                Caption = fileName,
                Url = url,
                ProfileId = profileId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Images.Add(image);
            await this._dbContext.SaveChangesAsync();

            return image.Id;
        }

        private void DeleteImage(string imageId) =>
            this._dbContext.Images.Remove(new Image { Id = imageId });
    }
}