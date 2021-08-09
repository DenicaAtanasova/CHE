namespace CHE.Services.Data.Tests.Mocks
{
    using Moq;

    public class ReviewsServiceMock
    {
        public static IReviewsService Instance
        {
            get
            {
                var reviewsService = new Mock<IReviewsService>();
                reviewsService.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(true);

                return reviewsService.Object;
            }
        }
    }
}