namespace CHE.Web.ViewModels.Teachers
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System;
    using System.Linq;

    public class TeacherAllViewModel : IMapExplicitly
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public int ReviewsCount { get; set; }

        //public string ImageUrl { get; set; }

        //public string SchoolLevel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CheUser, TeacherAllViewModel>()
                //.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => Math.Round(src.ReviewsReceived.Average(x => x.Rating), 1)))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.ReviewsReceived.Sum(x => x.Rating)))
                .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(src => src.ReviewsReceived.Count));
                //.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Portfolio.Image.Url))
                //.ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.Portfolio.SchoolLevel.ToString()));
        }
    }
}