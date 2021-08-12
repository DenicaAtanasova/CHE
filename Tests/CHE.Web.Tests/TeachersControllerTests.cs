namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Teachers;

    public class TeachersControllerTests
    {
        [Fact]
        public void AllShouldReturnViewWithCorrectTeachers() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Teachers/All?pageIndex=1")
                .To<TeachersController>(c => c.All(null, null, null, 1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModel(TeachersList));

        [Fact]
        public void DetailsShouldReturnViewWithTeacher() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Teachers/Details/id")
                .To<TeachersController>(c => c.Details("id"))
                .Which()
                .ShouldReturn()
                .View(view => view.WithModel(DetailsTeacher));

        [Fact]
        public void DetailsShouldReturnNotFound() =>
            MyMvc
                .Controller<TeachersController>()
                .Calling(c => c.Details(""))
                .ShouldReturn()
                .NotFound();
    }
}