namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    public class JoinRequestsServiceMock
    {
        public static IJoinRequestsService Instance
        {
            get
            {
                var joinRequestsService = new Mock<IJoinRequestsService>();
                joinRequestsService.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

                return joinRequestsService.Object;
            }
        }
    }
}