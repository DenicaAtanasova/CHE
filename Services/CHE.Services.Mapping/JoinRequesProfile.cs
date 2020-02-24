namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Cooperatives;

    public class JoinRequesProfile : Profile
    {
        public JoinRequesProfile()
        {
            this.CreateMap<JoinRequest, CooperativeJoinRequestViewModel>();
        }
    }
}
