﻿namespace CHE.Web.InputModels.Events
{
    using CHE.Web.InputModels.Attributes.Validation;

    using System;
    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Event;
    using static DataErrorMessages;

    public class EventCreateInputModel
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

        [Required]
        [Display(Name="Start date")]
        public DateTime StartDate { get; init; }

        [Required]
        [DateAfter(nameof(StartDate))]
        [Display(Name="End date")]
        public DateTime EndDate { get; init; }

        public string ScheduleId { get; init; }

        public string CooperativeId { get; init; }
    }
}