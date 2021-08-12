namespace CHE.Web.Tests
{
    using CHE.Web.Areas.Parent.Controllers;
    using CHE.Web.InputModels.Reviews;

    using MyTested.AspNetCore.Mvc;

    using System;

    using Xunit;

    using static CHE.Web.Tests.Data.Reviews;

    public class ParentReviewsControllerTests
    {
        [Fact]
        public void SendMethodGetShouldReturnViewWithCorrectReview() =>
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
        public void SendMethodGetShouldReturnNotFoundWithInvalidStringId() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Send(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void SendMethodPostShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Parent/Reviews/Send")
                    .WithMethod(HttpMethod.Post)
                    .WithUser(user => user.InRole("Parent")))
                .To<ReviewsController>(c => c.Send(With.Any<ReviewCreateInputModel>()));

        [Fact]
        public void SendMethodPostWithValidModelStateShouldRedirect()
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
        public void SenMethodPostWithInvalidStateShouldReturnSameView()
        {
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
        }

        [Fact]
        public void UpdateMethodGetShouldReturnViewWithCorrectViewModel() =>
           MyMvc
               .Pipeline()
               .ShouldMap(request => request
                   .WithPath("/Parent/Reviews/Update/id")
                   .WithUser(user => user.InRole("Parent")))
               .To<ReviewsController>(c => c.Update("id"))
               .Which()
               .WithDependencies(
                   MockProvider.ParentsService(), 
                   MockProvider.ReviewsService())
               .ShouldReturn()
               .View(view =>
                   view.WithModel(ReviewToUpdate));

        [Fact]
        public void UpdateMethodGetShouldReturnNotFoundWithInvalidStringId() =>
            MyMvc
                .Controller<ReviewsController>()
                .Calling(c => c.Update(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void UpdateMethodPostShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Parent/Reviews/Update")
                    .WithMethod(HttpMethod.Post)
                    .WithUser(user => user.InRole("Parent")))
                .To<ReviewsController>(c => c.Update(With.Any<ReviewUpdateInputModel>()));

        [Fact]
        public void UpdateMethodPostWithValidModelStateShouldRedirect()
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
        public void UpdateMethodPostWithInvalidStateShouldReturnSameView()
        {
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
        }

        [Fact]
        public void DeleteShouldRedirect()
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