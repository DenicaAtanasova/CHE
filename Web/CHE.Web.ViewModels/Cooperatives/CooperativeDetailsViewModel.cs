namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeDetailsViewModel : IMapExplicitly
    {
        public string Id { get; set; }

        public string ScheduleId { get; set; }

        public string Name { get; set; }

        public string Info { get; set; }

        public string Grade { get; set; }

        public int MembersCount { get; set; }

        public string AdminUserName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public string PendingRequestId { get; set; }

        public CooperativeAddressViewModel Address { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeDetailsViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));
        }
    }
}