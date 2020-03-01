namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Web.ViewModels.JoinRequests;
    using CHE.Web.ViewModels.Teachers;

    public class JoinRequesProfile : Profile
    {
        public JoinRequesProfile()
        {
            //TODO: check if is in use
            this.CreateMap<JoinRequest, JoinRequest>();

            this.CreateMap<JoinRequest, CooperativeJoinRequestDetailsViewModel>();

            this.CreateMap<JoinRequest, JoinRequestDetailsViewModel>();

            this.CreateMap<JoinRequest, TeacherJoinRequestVIewModel>();
        }
    }
}
