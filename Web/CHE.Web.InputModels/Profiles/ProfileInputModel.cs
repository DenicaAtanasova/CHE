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
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Education { get; set; }

        public string Experience { get; set; }

        public string Skills { get; set; }

        public string Interests { get; set; }

        [Display(Name = "School level")]
        public string SchoolLevel { get; set; }

        public IFormFile Image { get; set; }

        public AddressInputModel Address { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Profile, ProfileInputModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}