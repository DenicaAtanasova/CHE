namespace CHE.Web.Tests.Data
{
    using CHE.Web.InputModels.Events;
    using System;
    using System.Globalization;

    public class Events
    {
        public static EventUpdateInputModel UpdateEvent =>
            new EventUpdateInputModel
            {
                Id = "id"
            };

        public static EventCreateInputModel AddEvent =>
            new EventCreateInputModel
            {
                ScheduleId = "id", 
                StartDate = DateTime.ParseExact(EventDate, "d-M-yyyy", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact(EventDate, "d-M-yyyy", CultureInfo.InvariantCulture),
                CooperativeId = "cooperativeId"
            };

        public static string EventDate => "13-8-2021";
    }
}