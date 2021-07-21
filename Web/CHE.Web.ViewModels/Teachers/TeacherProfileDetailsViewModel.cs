namespace CHE.Web.ViewModels.Teachers
{
    using AutoMapper;

    using CHE.Services.Mapping;

    public class TeacherProfileDetailsViewModel : IMapExplicitly
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Education { get; init; }

        public string Experience { get; init; }

        public string Skills { get; init; }

        public string Interests { get; init; }

        public string SchoolLevel { get; init; }

        public string ImageUrl { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Profile, TeacherProfileDetailsViewModel>()
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.SchoolLevel.ToString()));
        }
    }
}