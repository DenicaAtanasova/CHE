namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeDetailsViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string ScheduleId { get; init; }

        public string Name { get; init; }

        public string Info { get; init; }

        public string Grade { get; init; }

        public int MembersCount { get; init; }

        public string AdminUserName { get; init; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public string PendingRequestId { get; set; }

        public CooperativeAddressViewModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeDetailsViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));
        }
    }
}