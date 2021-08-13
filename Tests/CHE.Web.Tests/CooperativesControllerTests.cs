namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Cooperatives;

    public class CooperativesControllerTests
    {
        [Fact]
        public void All_ShouldReturnViewWithCorrectCooperatves() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Cooperatives/All")
                    .WithQueryString("?pageIndex=1&level=First&City=Sofia&Neighbourhood=Vitosha"))
            .To<CooperativesController>(c => 
                c.All(CooperativesFilter.Level, CooperativesFilter.City, CooperativesFilter.Neighbourhood, 1))
            .Which()
            .ShouldHave()
            .TempData(data =>
                data.ContainingEntry("levelDisplayName", "grade"))
            .AndAlso()
            .ShouldReturn()
            .View(view =>
                view.WithModel(CooperativesList));

        [Fact]
        public void Details_ShouldReturnCorrectCooperative() =>
            MyMvc
                .Pipeline()
                .ShouldMap("/Cooperatives/Details/id")
                .To<CooperativesController>(c => c.Details("id"))
                .Which()
                .ShouldReturn()
                .View(view => view.WithModel(DetailsCooperative));

        [Fact]
        public void Details_ShouldReturnNotFound() =>
            MyMvc
                .Controller<CooperativesController>()
                .Calling(c => c.Details(""))
                .ShouldReturn()
                .NotFound();
    }
}