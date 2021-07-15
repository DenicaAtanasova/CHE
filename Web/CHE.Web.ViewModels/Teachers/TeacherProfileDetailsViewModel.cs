namespace CHE.Web.ViewModels.Teachers
{
    using AutoMapper;

    using CHE.Services.Mapping;

    public class TeacherProfileDetailsViewModel : IMapExplicitly
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
            configuration.CreateMap<Data.Models.Profile, TeacherProfileDetailsViewModel>()
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.SchoolLevel.ToString()));
        }
    }
}