namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Web.InputModels.Reviews;
    using CHE.Web.ViewModels.Reviews;

    using Moq;

    using static CHE.Web.Tests.Data.Reviews;

    public class MockProvider
    {
        public static IReviewsService ReviewsService()
        {
            var reviewsService = new Mock<IReviewsService>();

            reviewsService.Setup(x => x.GetAllByReceiverAsync<ReviewAllViewModel>(It.IsAny<string>()))
                .ReturnsAsync(AllRceiverReviews);

            reviewsService.Setup(x => x.GetByIdAsync<ReviewUpdateInputModel>(It.IsAny<string>()))
                .ReturnsAsync(ReviewToUpdate);

            return reviewsService.Object;
        }

        public static IParentsService ParentsService()
        {
            var parentsService = new Mock<IParentsService>();

            return parentsService.Object;
        }
    }
}