namespace CHE.Web.ViewModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System;

    public class EventViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsFullDay { get; set; }
    }
}