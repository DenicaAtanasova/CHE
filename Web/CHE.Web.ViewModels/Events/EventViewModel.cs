namespace CHE.Web.ViewModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System;

    public class EventViewModel : IMapFrom<Event>
    {
        public string Id { get; init; }
        
        public string Title { get; init; }

        public string Description { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public bool IsFullDay { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}