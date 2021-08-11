using CHE.Web.Tests.Data;

namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Reviews;

    using Moq;

    using static Reviews;

    public class MockProvider
    {
        public static IReviewsService ReviewsService()
        {
            var reviewsService = new Mock<IReviewsService>();
            reviewsService.Setup(x => x.GetAllByReceiverAsync<ReviewAllViewModel>(It.IsAny<string>()))
                .ReturnsAsync(AllRceiverReviews);               

            return reviewsService.Object;
        }
    }
}