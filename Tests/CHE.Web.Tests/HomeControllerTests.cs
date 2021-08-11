namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnCorrectView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void ErrorShouldReturnCorrectView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}