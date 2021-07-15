namespace CHE.Services.Storage
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileStorage
    {
        Task<string> UploadAsync(string fileName, Stream content);

        Task DeleteAsync(string fileName);
    }
}