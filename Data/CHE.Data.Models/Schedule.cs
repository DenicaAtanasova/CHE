namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    using System.Collections.Generic;

    public class Schedule : BaseModel<string>
    {
        public Schedule()
        {
            this.Events = new HashSet<Event>();
        }

        public ICollection<Event> Events { get; set; }

        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }

        public string OwnerId { get; init; }

        public Teacher Owner { get; init; }
    }
}