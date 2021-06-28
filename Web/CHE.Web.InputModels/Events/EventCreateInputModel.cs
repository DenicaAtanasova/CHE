namespace CHE.Web.InputModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class EventCreateInputModel : IMapTo<Event>
    {
        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        [DateAfter(nameof(StartDate))]
        public DateTime EndDate { get; set; }

        public bool IsFullDay { get; set; }

        public string ScheduleId { get; set; }
    }
}
