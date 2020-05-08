namespace CHE.Web.ViewModels.Teachers
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class TeacherPortfolioDetailsViewModel : IMapExplicitly
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        public string SchoolLevel { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Portfolio, TeacherPortfolioDetailsViewModel>()
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.SchoolLevel.ToString()));
        }
    }
}