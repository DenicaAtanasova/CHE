namespace CHE.Web.InputModels.Portfolios
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;

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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Profile, ProfileInputModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}