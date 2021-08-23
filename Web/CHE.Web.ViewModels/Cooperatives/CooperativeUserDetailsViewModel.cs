namespace CHE.Web.ViewModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeUserDetailsViewModel : IMapExplicitly
    {
        public string ParentId { get; set; }

        public string UserId { get; init; }

        public string UserName { get; init; }

        public string CooperativeId { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ParentCooperative, CooperativeUserDetailsViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Parent.User.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Parent.UserId));
        }
    }
}