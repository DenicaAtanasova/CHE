namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    public class ProfilesServiceMock
    {
        public static IProfilesService Instance =>
            new Mock<IProfilesService>().Object;
    }
}