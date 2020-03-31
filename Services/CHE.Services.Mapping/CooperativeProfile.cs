namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;
    using CHE.Web.ViewModels.JoinRequests;

    public class CooperativeProfile : Profile
    {
        public CooperativeProfile()
        {
            this.CreateMap<Cooperative, Cooperative>();
            
            this.CreateMap<Cooperative, CooperativeAllViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));

            this.CreateMap<Cooperative, CooperativeDetailsViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value))
                .ForMember(dest => dest.MembersCount, opt => opt.MapFrom(src => src.Members.Count));

            this.CreateMap<Cooperative, CooperativeUpdateInputModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value))
                .ReverseMap();

            //TODO check if have to be moved to jrprofile
            this.CreateMap<Cooperative, JoinRequestCooperativeSendViewModel>();

            this.CreateMap<Cooperative, CooperativeJoinRequestsViewModel>();

            this.CreateMap<Cooperative, CooperativeMembersViewModel>();
        }
    }
}