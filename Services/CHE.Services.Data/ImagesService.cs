namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using System.Threading.Tasks;

    using CHE.Data;

    public class ImagesService : IImagesService
    {
        private const string CONTAINER_NAME = "uploads";
        private const string DEFAULT_IMAGE_CAPTION = "Teacher_Avatar.png";

        private readonly string accessKey;
        private readonly IConfiguration _configuration;
        private readonly CheDbContext _dbContext;

        public ImagesService(IConfiguration configuration,
            CheDbContext dbContext)
        {
            this._configuration = configuration;
            this._dbContext = dbContext;
            this.accessKey = this._configuration.GetConnectionString("BlobConnection");
        }

        public async Task<bool> UpdateAsync(IFormFile imageFile, string portfolioId)
        {
            var imageToUpdate = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.PortfolioId == portfolioId);

            if (imageToUpdate.Caption != DEFAULT_IMAGE_CAPTION)
            {
                await this.DeleteInBlobAsync(imageToUpdate.Url, imageToUpdate.Caption);
            }
            
            var url = await this.UploadToBlobAsync(imageFile);

            imageToUpdate.Url = url;
            imageToUpdate.Caption = imageFile.FileName;

            var entity = this._dbContext.Update(imageToUpdate);

            return true;
        }

        private async Task<string> UploadToBlobAsync(IFormFile imageFile)
        {
            var cloudBlockBlob = this.GetCloudBlockBlob(imageFile.FileName);
            cloudBlockBlob.Properties.ContentType = imageFile.ContentType;

            var imageStream = imageFile.OpenReadStream();
            await cloudBlockBlob.UploadFromStreamAsync(imageStream);

            var url = cloudBlockBlob.Uri.AbsoluteUri;

            return url;
        }

        private async Task<bool> DeleteInBlobAsync(string url, string fileName)
        {
            var cloudBlockBlob = this.GetCloudBlockBlob(fileName);
            var result = await cloudBlockBlob.DeleteIfExistsAsync();

            return result;
        }

        private CloudBlockBlob GetCloudBlockBlob(string fileName)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(CONTAINER_NAME);
            //var isContainerCreated = await cloudBlobContainer.CreateIfNotExistsAsync();
            //if (isContainerCreated)
            //{
            //    await cloudBlobContainer
            //        .SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            //}

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            return cloudBlockBlob;
        }
    }
}