namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System;

    public class Event : BaseModel<string>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsFullDay { get; set; }

        public string ScheduleId { get; set; }

        public Schedule Schedule { get; set; }
    }
}