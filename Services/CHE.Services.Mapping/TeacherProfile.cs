namespace CHE.Services.Mapping
{
    using System;
    using System.Linq;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Teachers;

    using AutoMapper;

    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            this.CreateMap<CheUser, TeacherAllViewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.ReviewsReceived.Average(x => x.Rating), 1)))
                .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(src => src.ReviewsReceived.Count))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Portfolio.Image.Url))
                .ForMember(dest => dest.EducationLevel, opt => opt.MapFrom(src => src.Portfolio.SchoolLevel.ToString()));

            this.CreateMap<CheUser, TeacherDetailsViewModel>();

            this.CreateMap<CheUser, TeacherReviewsViewModel>();
        }
    }
}