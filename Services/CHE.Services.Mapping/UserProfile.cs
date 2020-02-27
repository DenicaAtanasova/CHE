namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Cooperatives;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CheUserCooperative, CooperativeUserDetailsViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CheUser.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CheUserId))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.CheUser.RoleName));
        }
    }
}