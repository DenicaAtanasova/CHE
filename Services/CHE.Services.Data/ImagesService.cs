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
        private readonly string accessKey;
        private readonly IConfiguration _configuration;
        private readonly CheDbContext _dbContext;

        public ImagesService(IConfiguration configuration,
            CheDbContext dbContext)
        {
            this._configuration = configuration;
            this._dbContext = dbContext;
            this.accessKey = this._configuration.GetConnectionString("BlobConnection:AccessKey");
        }

        public async Task<bool> Update(IFormFile imageFile, string portfolioId)
        {
            var imageToUpdate = await this._dbContext.Images
                .SingleOrDefaultAsync(x => x.PortfolioId == portfolioId);

            var url = await this.UploadAsync(imageFile);
            if (url == null)
            {
                return false;
            }
            imageToUpdate.Url = url;
            imageToUpdate.Caption = imageFile.FileName;

            var entity = this._dbContext.Update(imageToUpdate);

            return true;
        }

        private async Task<string> UploadAsync(IFormFile imageFile)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            var strContainerName = "uploads";
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            var isContainerCreated = await cloudBlobContainer.CreateIfNotExistsAsync();
            if (isContainerCreated)
            {
                await cloudBlobContainer
                    .SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageFile.FileName);
            cloudBlockBlob.Properties.ContentType = imageFile.ContentType;

            var imageStream = imageFile.OpenReadStream();
            await cloudBlockBlob.UploadFromStreamAsync(imageStream);

            var url = cloudBlockBlob.Uri.AbsoluteUri;

            return url;
        }
    }
}