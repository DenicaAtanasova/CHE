namespace CHE.Web.ViewModels.Events
{
    using System;

    public class EventViewModel
    {
        public string Id { get; set; }
        
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}