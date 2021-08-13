namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Teachers;

    public class TeachersControllerTests
    {
        [Fact]
        public void All_ShouldReturnViewWithCorrectTeachers() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Teachers/All")
                    .WithQueryString("?pageIndex=1&level=First&City=Sofia&Neighbourhood=Vitosha"))
                .To<TeachersController>(c => 
                    c.All(TeachersFilter.Level, TeachersFilter.City, TeachersFilter.Neighbourhood, 1))
                .Which()
                .ShouldHave()
                .TempData(data => 
                    data.ContainingEntry("levelDisplayName", "school level"))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModel(TeachersList));

        [Fact]
        public void Details_ShouldReturnViewWithTeacher() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Teachers/Details/id")
                .To<TeachersController>(c => c.Details("id"))
                .Which()
                .ShouldReturn()
                .View(view => view.WithModel(DetailsTeacher));

        [Fact]
        public void Details_ShouldReturnNotFound() =>
            MyMvc
                .Controller<TeachersController>()
                .Calling(c => c.Details(""))
                .ShouldReturn()
                .NotFound();
    }
}