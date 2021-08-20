namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using static CHE.Common.DataConstants.Event;

    public class Event : BaseModel<string>
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public string ScheduleId { get; init; }

        public Schedule Schedule { get; init; }
    }
}