namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    public class CooperativesServiceMock
    {
        public static ICooperativesService Instance =>
            new Mock<ICooperativesService>().Object;
    }
}