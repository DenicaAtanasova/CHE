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
            this.CreateMap<JoinRequest, CooperativeJoinRequestViewModel>();

            this.CreateMap<JoinRequest, JoinRequestDetailsViewModel>();
        }
    }
}
