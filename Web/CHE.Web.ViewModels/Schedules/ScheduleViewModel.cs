namespace CHE.Web.ViewModels.Schedules
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class ScheduleViewModel : IMapFrom<Schedule>
    {
        public string Id { get; init; }

        public string CooperativeId { get; init; }

        public string TeacherId { get; init; }
    }
}