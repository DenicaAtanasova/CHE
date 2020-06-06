namespace CHE.Web.ViewModels.Schedules
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class ScheduleViewModel : IMapFrom<Schedule>
    {
        public string Id { get; set; }

        public string CooperativeId { get; set; }

        public string TeacherId { get; set; }
    }
}