namespace CHE.Services.Data.Tests.Mocks
{
    using CHE.Services.Storage;

    using Moq;
    using System.IO;

    using static MockConstants;

    public class CloudStorageServiceMock
    {
        public static IFileStorage Instance
        {
            get
            {
                var cloudStorageService = new Mock<IFileStorage>();
                cloudStorageService.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                    .ReturnsAsync(CloudStorageMock.Url);

                return cloudStorageService.Object;
            }
        }
    }
}