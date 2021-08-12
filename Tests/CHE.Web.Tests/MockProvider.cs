namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Services.Storage;
    using CHE.Web.InputModels.Reviews;
    using CHE.Web.ViewModels.Messengers;
    using CHE.Web.ViewModels.Reviews;
    using CHE.Web.ViewModels.Teachers;

    using Moq;

    using System.Linq;

    using static CHE.Web.Tests.Data.Reviews;
    using static CHE.Web.Tests.Data.Teachers;
    using static CHE.Web.Tests.Data.Messengers;

    public class MockProvider
    {
        public static IReviewsService ReviewsService()
        {
            var reviewsService = new Mock<IReviewsService>();

            reviewsService
                .Setup(x => x.GetAllByReceiverAsync<ReviewAllViewModel>(It.IsAny<string>()))
                .ReturnsAsync(AllRceiverReviews);

            reviewsService
                .Setup(x => x.GetByIdAsync<ReviewUpdateInputModel>("id"))
                .ReturnsAsync(ReviewToUpdate);

            return reviewsService.Object;
        }

        public static IParentsService ParentsService()
        {
            var parentsService = new Mock<IParentsService>();

            return parentsService.Object;
        }

        public static ITeachersService TeachersService()
        {
            var teachersService = new Mock<ITeachersService>();

            teachersService
                .Setup(x => x.GetAllAsync<TeacherAllViewModel>(
                    It.IsAny<int>(),
                    It.IsAny<int>(), 
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(AllTeachers);

            teachersService
                .Setup(x => x.CountAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(AllTeachers.Count());

            teachersService
                .Setup(x => x.GetByIdAsync<TeacherDetailsViewModel>("id"))
                .ReturnsAsync(DetailsTeacher);

            return teachersService.Object;
        }

        public static IFileStorage CloudStorageService() =>
            new Mock<IFileStorage>().Object;

        public static IMessengersService MessengersService()
        {
            var messengersService = new Mock<IMessengersService>();

            messengersService
                .Setup(x => x.GetAllPrivateMessengersByUserAsync<MessengerUserViewModel>(It.IsAny<string>()))
                .ReturnsAsync(AllPrivateMessengers);

            messengersService
                .Setup(x => x.GetPrivateMessengerAsync<MessengerPrivateViewModel>(It.IsAny<string>(), "receiverId"))
                .ReturnsAsync(CurrentMessenger);

            return messengersService.Object;
        }
    }
}