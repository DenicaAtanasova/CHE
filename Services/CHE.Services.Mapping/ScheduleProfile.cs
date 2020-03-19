namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Schedules;

    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            this.CreateMap<Schedule, ScheduleViewModel>();
        }
    }
}
