namespace CHE.Web.Tests
{
    using CHE.Web.Areas.Parent.Controllers;
    using CHE.Web.InputModels.Cooperatives;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Cooperatives;

    public class ParentCooperativesControllerTests
    {
        [Fact]
        public void Create_ShouldReturnView() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Parent/Cooperatives/Create")
                    .WithUser(user => user.InRole("Parent")))
                .To<CooperativesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void Create_MethodPost_ShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Parent/Cooperatives/Create")
                    .WithUser(user => user.InRole("Parent"))
                    .WithMethod(HttpMethod.Post))
                .To<CooperativesController>(c => c.Create(new CooperativeCreateInputModel()));

        [Fact]
        public void Create_MethodPost_WithInvalidModelState_ShouldReturnSameView() =>
            MyMvc
                .Controller<CooperativesController>()
                .WithUser(user => user.InRole("Parent"))
                .Calling(c => c.Create(new CooperativeCreateInputModel()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void Create_MethodPost_WithInvalidModelState_ShouldRedirect() =>
            MyMvc
                .Controller<CooperativesController>()
                .WithUser(user => user.InRole("Parent"))
                .Calling(c => c.Create(CreateCooperative))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result => 
                    result.To<Controllers.CooperativesController>(c => 
                        c.All(null, null, null, With.No<int>())));

        [Fact]
        public void Update_MethodGet_ShouldReturnNotFound() =>
           MyMvc
               .Controller<CooperativesController>()
                .Calling(c => c.Update(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void Update_MethodGet_ShouldReturnViewWithCorrectViewModel() =>
           MyMvc
               .Pipeline()
               .ShouldMap(request => request
                   .WithPath("/Parent/Cooperatives/Update/id")
                   .WithUser(user => user.InRole("Parent")))
               .To<CooperativesController>(c => c.Update("id"))
               .Which()
               .ShouldReturn()
               .View(view =>
                   view.WithModel(UpdateCooperative));

        [Fact]
        public void Update_MethodPost_ShouldReturnSameView() =>
            MyMvc
                .Controller<CooperativesController>()
                .Calling(c => c.Update(new CooperativeUpdateInputModel()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => 
                    view.WithModelOfType<CooperativeUpdateInputModel>());

        [Fact]
        public void Update_MethodPost_ShouldRedirect() =>
            MyMvc
                .Controller<CooperativesController>()
                .Calling(c => c.Update(UpdateCooperative))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result =>
                    result.To<Controllers.CooperativesController>(c =>
                        c.Details("id")));

        [Fact]
        public void Delete_ShouldRedirect() =>
            MyMvc
                .Controller<CooperativesController>()
                .WithUser(user => user.InRole("Parent"))
                .Calling(c => c.Delete("id"))
                .ShouldReturn()
                .Redirect(result =>
                    result.To<Controllers.CooperativesController>(c =>
                        c.All(null, null, null, With.No<int>())));

    }
}