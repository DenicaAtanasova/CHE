namespace CHE.Web.ViewModels.JoinRequests
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestCooperativeAllViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string Sender { get; init; }

        public string Admin { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<JoinRequest, JoinRequestCooperativeAllViewModel>()
                .ForMember(dest => dest.Admin, opt =>
                    opt.MapFrom(src => src.Cooperative.Admin.User.UserName))
                .ForMember(dest => dest.Sender, opt =>
                    opt.MapFrom(src => src.Sender.User.UserName));
        }
    }
}