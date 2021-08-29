namespace CHE.Web.Tests
{
    using System;
    using System.Collections.Generic;

    using CHE.Web.Controllers;
    using CHE.Web.InputModels.Events;
    using CHE.Web.ViewModels.Events;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static CHE.Web.Tests.Data.Events;

    public class EventsControllerTests
    {
        [Fact]
        public void GetThreeMonthsEvents_ShouldReturnJsonWithCorrectViewModel() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Schedule/Events/GetThreeMonthsEvents/scheduleId/date")
                    .WithUser())
                .To<EventsController>(c => c.GetThreeMonthsEvents("scheduleId", "date"))
                .Which()
                .ShouldReturn()
                .Json(result =>
                    result.WithModelOfType<Dictionary<string, EventViewModel[]>>());

        [Fact]
        public void Delete_ShouldRedirect() =>
            MyMvc
                .Controller<EventsController>()
                .Calling(x => x.Delete("id", "scheduleId"))
                .ShouldReturn()
                .Redirect(result => result
                    .To<SchedulesController>(c => c.MyDetails("scheduleId")));

        [Fact]
        public void Delete_ShouldReturnNotFound() =>
            MyMvc
                .Controller<EventsController>()
                .Calling(x => x.Delete("", "scheduleId"))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void Details_ShouldReturnViewWithCorrectEvent() =>
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Schedule/Events/Details/id")
                    .WithUser())
                .To<EventsController>(c => c.Details("id"))
                .Which()
                .ShouldReturn()
                .View(result =>
                    result.WithModel(UpdateEvent));

        [Fact]
        public void Details_MethodGet_ShouldReturnNotFound() =>
            MyMvc
                .Controller<EventsController>()
                .Calling(c => c.Details(""))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void Update_MethodPost_ShouldMapCorrectRoute() =>
            MyMvc
                .Routing()
                .ShouldMap(request => request
                    .WithPath("/Schedule/Events/Update/id")
                    .WithMethod(HttpMethod.Post)
                    .WithUser())
                .To<EventsController>(c => c.Update("id", null));

        [Fact]
        public void Update_MethodPost_WithValidModelState_ShouldRedirect()
        {
            var scheduleId = "scheduleId";

            MyMvc
                .Controller<EventsController>()
                .Calling(c => c.Update("id", new EventUpdateInputModel
                {
                    Title = "Title",
                    Description = "Description",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(30),
                    ScheduleId = scheduleId
                }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result => result
                    .To<SchedulesController>(c => c.Details(scheduleId)));
        }

        [Fact]
        public void Update_MethodPost_WithInValidModelState_ShouldReturnSameView() =>
            MyMvc
                .Controller<EventsController>()
                .Calling(c => c.Update("id", new EventUpdateInputModel()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view =>
                    view.WithModelOfType<EventUpdateInputModel>());

        [Fact]
        public void Add_MethodGet_ShouldReturnViewWithCorrectEvent() =>
           MyMvc
               .Pipeline()
               .ShouldMap(request => request
                   .WithPath($"/Schedule/Events/Add/id/{EventDate}")
                   .WithUser())
               .To<EventsController>(c => c.Add("id", EventDate))
               .Which()
               .ShouldReturn()
               .View(result =>
                   result.WithModel(AddEvent));

        [Fact]
        public void Add_MethodGet_ShouldReturnNotFound() =>
           MyMvc
               .Controller<EventsController>()
               .Calling(c => c.Add("", ""))
               .ShouldReturn()
               .NotFound();

        [Fact]
        public void Add_MethodPost_WithValidModelState_ShouldRedirect()
        {
            var scheduleId = "id";

            MyMvc
                .Controller<EventsController>()
                .Calling(c => c.Add(new EventCreateInputModel
                {
                    Title = "Title",
                    Description = "Description",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(30),
                    ScheduleId = scheduleId
                }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(result => result
                    .To<SchedulesController>(c => c.Details(scheduleId)));
        }

        [Fact]
        public void Add_MethodPost_WithInValidModelState_ShouldReturnSameView() =>
            MyMvc
                .Controller<EventsController>()
                .Calling(c => c.Add(new EventCreateInputModel
                { 
                    ScheduleId = "id"
                }))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view =>
                    view.WithModelOfType<EventCreateInputModel>());
    }
}