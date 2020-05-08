namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeUserDetailsViewModel : IMapExplicitly
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string CooperativeId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CheUserCooperative, CooperativeUserDetailsViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CheUser.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CheUserId))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.CheUser.RoleName));
        }
    }
}