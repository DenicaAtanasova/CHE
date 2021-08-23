namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;
    using CHE.Web.ViewModels.Messengers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Messengers;

    public class MessengersControllerTests
    {
        [Fact]
        public void Details_ShouldReturnViewWithCorrectMessenger() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Messengers/Details")
                    .WithQueryString("?currentMessengerId=messengerId&receiverId=receiverId")
                    .WithUser())
                .To<MessengersController>(c => c.Details("messengerId", "receiverId"))
                .Which()
                .ShouldReturn()
                .View(view => view.WithModel(MessengerUsers));

        [Fact]
        public void GetPrivate_ShouldReturnJsonWithCorrectModel() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Messengers/GetPrivate")
                    .WithQueryString("?receiverId=receiverId")
                    .WithUser())
                .To<MessengersController>(c => c.GetPrivate("receiverId"))
                .Which()
                .ShouldReturn()
                .Json(result => result.WithModelOfType<MessengerPrivateViewModel>());

    }
}