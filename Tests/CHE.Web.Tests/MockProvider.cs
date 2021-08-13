namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Services.Storage;

    using CHE.Web.InputModels.Reviews;
    using CHE.Web.ViewModels.Messengers;
    using CHE.Web.ViewModels.Reviews;
    using CHE.Web.ViewModels.Teachers;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Web.ViewModels.Schedules;
    using CHE.Web.InputModels.Events;

    using Moq;

    using System.Linq;

    using static CHE.Web.Tests.Data.Reviews;
    using static CHE.Web.Tests.Data.Teachers;
    using static CHE.Web.Tests.Data.Messengers;
    using static CHE.Web.Tests.Data.Cooperatives;
    using static CHE.Web.Tests.Data.Schedules;
    using static CHE.Web.Tests.Data.Events;
    using CHE.Web.ViewModels.Events;

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
                .Setup(x => x.GetAllPrivateContactsByUserAsync<MessengerUserViewModel>(It.IsAny<string>()))
                .ReturnsAsync(AllPrivateMessengers);

            messengersService
                .Setup(x => x.GetPrivateMessengerAsync<MessengerPrivateViewModel>(It.IsAny<string>(), "receiverId"))
                .ReturnsAsync(CurrentMessenger);

            return messengersService.Object;
        }

        public static IJoinRequestsService JoinRequestsService()
        {
            var joinRequestsService = new Mock<IJoinRequestsService>();

            joinRequestsService
                .Setup(x => x.GetPendindRequestIdAsync(It.IsAny<string>(), "id"))
                .ReturnsAsync("requestId");

            return joinRequestsService.Object;
        }

        public static ICooperativesService CooperativesService()
        {
            var cooperativesService = new Mock<ICooperativesService>();

            cooperativesService
                .Setup(x => x.GetByIdAsync<CooperativeDetailsViewModel>("id"))
                .ReturnsAsync(DetailsCooperative);

            cooperativesService
                .Setup(x => x.CheckIfAdminAsync(It.IsAny<string>(), "id"))
                .ReturnsAsync(false);

            cooperativesService
                .Setup(x => x.CheckIfMemberAsync(It.IsAny<string>(), "id"))
                .ReturnsAsync(false);

            cooperativesService
                .Setup(x => x.GetAllAsync<CooperativeAllViewModel>(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(AllCooperatives);

            cooperativesService
                .Setup(x => x.CountAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(AllCooperatives.Count());

            return cooperativesService.Object;
        }

        public static ISchedulesService SchedulesService()
        {
            var schedulesService = new Mock<ISchedulesService>();

            schedulesService
                .Setup(x => x.GetByIdAsync<ScheduleViewModel>("id"))
                .ReturnsAsync(DetailsSchedule);

            schedulesService
               .Setup(x => x.GetByIdAsync<EventScheduleViewModel>("id"))
               .ReturnsAsync(EventSchedule);

            return schedulesService.Object;
        }

        public static IEventsService EventsService()
        {
            var eventsService = new Mock<IEventsService>();

            eventsService
                .Setup(x => x.GetByIdAsync<EventUpdateInputModel>("id"))
                .ReturnsAsync(UpdateEvent);

            return eventsService.Object;
        }
    }
}