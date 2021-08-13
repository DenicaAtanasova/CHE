namespace CHE.Web.Tests
{
    using CHE.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Schedules;

    public class SchedulesControllerTests
    {
        [Fact]
        public void DetailsShouldReturnCorrectSchedule() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Schedule/Details")
                    .WithQueryString("?id=id")
                    .WithUser())
                .To<SchedulesController>(c => c.Details("id"))
                .Which()
                .ShouldHave()
                .ViewData(data => 
                    data.ContainingEntry("id", DetailsSchedule.CooperativeId))
                .AndAlso()
                .ShouldReturn()
                .View(view =>
                    view.WithModel(DetailsSchedule));

        [Fact]
        public void DetailsShouldReturnNotFound() =>
            MyMvc
                .Controller<SchedulesController>()
                .Calling(c => c.Details(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void MyDetailsShouldReturnCorrectSchedule() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Schedule/MyDetails")
                    .WithQueryString("?id=id")
                    .WithUser())
                .To<SchedulesController>(c => c.MyDetails("id"))
                .Which()
                .ShouldReturn()
                .View(view =>
                    view.WithModel(DetailsSchedule));

        [Fact]
        public void MyDetailsShouldReturnNotFound() =>
           MyMvc
               .Controller<SchedulesController>()
               .Calling(c => c.MyDetails(""))
               .ShouldReturn()
               .NotFound();
    }
}
