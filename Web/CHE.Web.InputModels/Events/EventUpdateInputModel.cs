namespace CHE.Web.InputModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class EventUpdateInputModel : IMapTo<Event>
    {
        public string Id { get; init; }

        [Required]
        [StringLength(20)]
        public string Title { get; init; }

        public string Description { get; init; }

        public DateTime StartDate { get; init; }

        [DateAfter(nameof(StartDate))]
        public DateTime EndDate { get; init; }

        public bool IsFullDay { get; init; }

        public string ScheduleId { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
