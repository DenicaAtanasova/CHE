namespace CHE.Web.ViewModels.Events
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class EventScheduleViewModel :IMapFrom<Schedule>
    {
        public string CooperativeId { get; set; }
    }
}