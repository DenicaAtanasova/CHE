namespace CHE.Web.InputModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Event;
    using static DataErrorMessages;

    public class EventCreateInputModel : IMapTo<Event>
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Title { get; init; }

        [Required]
        [StringLength(
            DescriptionMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Description { get; init; }

        public DateTime StartDate { get; init; }

        [DateAfter(nameof(StartDate))]
        public DateTime EndDate { get; init; }

        public bool IsFullDay { get; init; }

        public string ScheduleId { get; init; }
    }
}
