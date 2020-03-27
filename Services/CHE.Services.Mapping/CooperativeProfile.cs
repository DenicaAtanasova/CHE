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

            this.CreateMap<CooperativeCreateInputModel, Cooperative>();

            this.CreateMap<Cooperative, CooperativeAllViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));

            this.CreateMap<Cooperative, CooeprativeDetailsViewModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value))
                .ForMember(dest => dest.MembersCount, opt => opt.MapFrom(src => src.Members.Count));

            this.CreateMap<Cooperative, CooperativeUpdateInputModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value))
                .ReverseMap();

            this.CreateMap<Cooperative, JoinRequestCooperativeSendViewModel>();
        }
    }
}