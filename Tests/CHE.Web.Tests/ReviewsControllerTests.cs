namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;
    using CHE.Web.ViewModels.Reviews;

    using MyTested.AspNetCore.Mvc;

    using System.Collections.Generic;

    using Xunit;

    using static CHE.Web.Tests.Data.Reviews;

    public class ReviewsControllerTests
    {
        [Fact]
        public void All_ShouldReturnViewWithCorrectViewModel() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Teachers/Reviews/someId")
                .To<ReviewsController>(c => c.All("someId"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<ReviewAllViewModel>>());

        [Fact]
        public void All_ShouldReturnViewWithCorrectReviews() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.All("receiverId"))
                .ShouldReturn()
                .View(view => view.WithModel(AllRceiverReviews));

        [Fact]
        public void MyAll_ShouldReturnViewWithCorrectViewModel() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Identity/Account/Manage/Reviews")
                .To<ReviewsController>(c => c.MyAll())
                .Which()
                .ShouldHave()
                .ActionAttributes(attribute => 
                    attribute.RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithName("All")
                    .WithModelOfType<IEnumerable<ReviewAllViewModel>>());

        [Fact]
        public void MyAll_ShouldReturnViewWithCorrectReviews() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.MyAll())
                .ShouldReturn()
                .View(view => view
                    .WithName("All")
                    .WithModel(AllRceiverReviews));
    }
}