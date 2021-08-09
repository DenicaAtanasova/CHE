namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    public class ImagesServiceMock
    {
        public static IImagesService Instanse =>
            new Mock<IImagesService>().Object;
    }
}