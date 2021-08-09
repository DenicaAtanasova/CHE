namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    using static MockConstants;

    public class GradesServiceMock
    {
        public static IGradesService Instance
        {
            get
            {
                var gradesService = new Mock<IGradesService>();
                gradesService.Setup(x => x.GetGardeIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GradeMock.Id);

                return gradesService.Object;
            }
        }
    }
}