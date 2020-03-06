namespace CHE.Services.Mapping
{
    using System.Linq;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Teachers;

    using AutoMapper;
    using System;

    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            this.CreateMap<CheUser, TeacherAllViewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.ReviewsReceived.Average(x => x.Rating), 1)))
                .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(src => src.ReviewsReceived.Count));

            this.CreateMap<CheUser, TeacherDetailsViewModel>();
        }
    }
}