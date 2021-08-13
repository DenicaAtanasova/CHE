namespace CHE.Web.Tests
{
    using CHE.Web.Areas.Parent.Controllers;
    using CHE.Web.InputModels.Reviews;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Reviews;

    public class ParentReviewsControllerTests
    {
        [Fact]
        public void Send_MethodGet_ShouldReturnViewWithCorrectReview() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Parent/Reviews/Send/id")
                    .WithUser(user => user.InRole("Parent")))
                .To<ReviewsController>(c => c.Send("id"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewCreateInputModel>()
                    .Passing(model => model.ReceiverId == "id"));

        [Fact]
        public void Send_MethodGet_WithInvalidStringId_ShouldReturnNotFound() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Send(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void Send_MethodPost_ShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Parent/Reviews/Send")
                    .WithMethod(HttpMethod.Post)
                    .WithUser(user => user.InRole("Parent")))
                .To<ReviewsController>(c => c.Send(With.Any<ReviewCreateInputModel>()));

        [Fact]
        public void Send_MethodPost_WithValidModelState_ShouldRedirect()
        {
            var receiverId = "id";

            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Send(new ReviewCreateInputModel
                {
                    Comment = "Coment",
                    Rating = 3,
                    ReceiverId = receiverId
                }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result => result
                    .To<Controllers.ReviewsController>(c => c.All(receiverId)));
        }

        [Fact]
        public void Send_MethodPost_WithInvalidState_ShouldReturnSameView() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Send(new ReviewCreateInputModel
                {
                    Comment = "Coment",
                    Rating = 3
                }))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view =>
                    view.WithModelOfType<ReviewCreateInputModel>());

        [Fact]
        public void Update_MethodGet_ShouldReturnViewWithCorrectViewModel() =>
           MyMvc
               .Pipeline()
               .ShouldMap(request => request
                   .WithPath("/Parent/Reviews/Update/id")
                   .WithUser(user => user.InRole("Parent")))
               .To<ReviewsController>(c => c.Update("id"))
               .Which()
               .ShouldReturn()
               .View(view =>
                   view.WithModel(ReviewToUpdate));

        [Fact]
        public void Update_MethodGet_WithInvalidStringId_ShouldReturnNotFound() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Update(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void Update_MethodPost_ShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Parent/Reviews/Update")
                    .WithMethod(HttpMethod.Post)
                    .WithUser(user => user.InRole("Parent")))
                .To<ReviewsController>(c => c.Update(With.Any<ReviewUpdateInputModel>()));

        [Fact]
        public void Update_MethodPost_WithValidModelState_ShouldRedirect()
        {
            var receiverId = "id";

            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Send(new ReviewCreateInputModel
                {
                    Comment = "Coment",
                    Rating = 3,
                    ReceiverId = receiverId
                }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result => result
                    .To<Controllers.ReviewsController>(c => c.All(receiverId)));

        }

        [Fact]
        public void Update_MethodPost_WithInvalidState_ShouldReturnSameView() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Update(new ReviewUpdateInputModel
                {
                    Rating = 3
                }))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view =>
                    view.WithModelOfType<ReviewUpdateInputModel>());

        [Fact]
        public void Delete_ShouldRedirect()
        {
            var receiverId = "receiverId";

            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Delete("reviewId", receiverId))
                .ShouldReturn()
                .Redirect(result => result
                    .To<Controllers.TeachersController>(c => c.Details(receiverId)));
        }
    }
}