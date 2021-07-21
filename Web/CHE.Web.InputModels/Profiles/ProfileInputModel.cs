namespace CHE.Web.InputModels.Profiles
{
    using AutoMapper;

    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Addresses;

    using Microsoft.AspNetCore.Http;

    using System.ComponentModel.DataAnnotations;

    public class ProfileInputModel : IMapExplicitly
    {
        [Display(Name = "First name")]
        public string FirstName { get; init; }

        [Display(Name = "Last name")]
        public string LastName { get; init; }

        public string Education { get; init; }

        public string Experience { get; init; }

        public string Skills { get; init; }

        public string Interests { get; init; }

        [Display(Name = "School level")]
        public string SchoolLevel { get; init; }

        public IFormFile Image { get; init; }

        public AddressInputModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Profile, ProfileInputModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}