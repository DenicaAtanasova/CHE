namespace CHE.Web.Tests.Data
{
    using CHE.Web.ViewModels.Events;
    using CHE.Web.ViewModels.Schedules;

    public class Schedules
    {
        public static ScheduleViewModel DetailsSchedule =>
            new ScheduleViewModel
            {
                CooperativeId = "cooperativeId"
            };

        public static EventScheduleViewModel EventSchedule =>
            new EventScheduleViewModel
            {
                CooperativeId = "cooperativeId"
            };
    }
}