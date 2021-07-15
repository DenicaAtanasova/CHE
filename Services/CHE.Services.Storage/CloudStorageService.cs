namespace CHE.Services.Storage
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;

    using System.IO;
    using System.Threading.Tasks;

    public class CloudStorageService : IFileStorage
    {
        private readonly string _containerName = "uploads";
        private readonly BlobContainerClient _container;

        public CloudStorageService(string connectionString)
        {
            this._container = new BlobContainerClient(connectionString, _containerName);
        }

        public async Task DeleteAsync(string fileName)
        {
            var blob = this._container.GetBlobClient(fileName);

            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public async Task<string> UploadAsync(string fileName, Stream content)
        {
            //TODO: Check if blob exists
            var blob = this._container.GetBlobClient(fileName);

            await blob.UploadAsync(content);

            return blob.Uri.AbsoluteUri;
        }
    }
}