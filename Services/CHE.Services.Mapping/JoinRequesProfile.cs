namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Web.ViewModels.JoinRequests;

    public class JoinRequesProfile : Profile
    {
        public JoinRequesProfile()
        {
            this.CreateMap<JoinRequest, JoinRequest>();

            this.CreateMap<JoinRequest, CooperativeJoinRequestDetailsViewModel>();

            this.CreateMap<JoinRequest, JoinRequestDetailsViewModel>();
        }
    }
}
