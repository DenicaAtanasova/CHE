namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Events;

    public class EventProfile : Profile
    {
        public EventProfile()
        {
            this.CreateMap<Event, EventViewModel>();
        }
    }
}
