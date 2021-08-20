namespace CHE.Web.InputModels.Profiles
{
    using AutoMapper;

    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    using Microsoft.AspNetCore.Http;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Profile;
    using static DataErrorMessages;

    public class ProfileInputModel : IMapExplicitly
    {
        public string Id { get; set; }

        [Display(Name = "First name")]
        [StringLength(
            FirstNameMaxLength,
            MinimumLength = FirstNameMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string FirstName { get; init; }

        [Display(Name = "Last name")]
        [StringLength(
            LastNameMaxLength,
            MinimumLength = LastNameMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string LastName { get; init; }

        [StringLength(
            EducationMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Education { get; init; }

        [StringLength(
            ExperienceMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Experience { get; init; }

        [StringLength(
            SkillsMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Skills { get; init; }

        [StringLength(
            InterestsMaxLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Interests { get; init; }

        [Display(Name = "School level")]
        [SchoolLevel]
        public string SchoolLevel { get; init; }

        public IFormFile Image { get; init; }

        public ProfileAddressInputModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Profile, ProfileInputModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}