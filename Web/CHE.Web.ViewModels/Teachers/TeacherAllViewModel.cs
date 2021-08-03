namespace CHE.Web.ViewModels.Teachers
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System;
    using System.Linq;

    public class TeacherAllViewModel : IMapExplicitly
    {
        public string Id { get; init; }

        public string UserUserName { get; init; }

        public double Rating { get; init; }

        public int ReviewsCount { get; init; }

        public string ImageUrl { get; init; }

        public string SchoolLevel { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Teacher, TeacherAllViewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(
                    src => src.ReviewsReceived.Count == 0 ?
                                                        0:
                                                        Math.Round(src.ReviewsReceived.Average(x => x.Rating), 1)))
                .ForMember(dest => dest.ReviewsCount, opt => opt.MapFrom(src => src.ReviewsReceived.Count))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Profile.Image.Url))
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.Profile.SchoolLevel.ToString()));
        }
    }
}