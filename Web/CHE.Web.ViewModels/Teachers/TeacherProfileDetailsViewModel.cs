namespace CHE.Web.ViewModels.Teachers
{
    using autoMapper =  AutoMapper;

    using CHE.Data.Models;
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

        public void CreateMappings(autoMapper.IProfileExpression configuration)
        {
            configuration.CreateMap<Profile, TeacherProfileDetailsViewModel>()
                .ForMember(dest => dest.SchoolLevel, opt => opt.MapFrom(src => src.SchoolLevel.ToString()));
        }
    }
}