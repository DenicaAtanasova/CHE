namespace CHE.Web.InputModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Cooperative;
    using static DataErrorMessages;

    public class CooperativeUpdateInputModel : IMapExplicitly
    {
        public string Id { get; init; }

        [Required]
        [StringLength(
             NameMaxLength,
             MinimumLength = NameMinLength,
             ErrorMessage = StringLengthErroMessage)]
        public string Name { get; init; }

        [Required]
        [StringLength(
            InfoMaxLength,
            MinimumLength = InfoMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Info { get; init; }

        [Required]
        [Grade]
        public string Grade { get; init; }

        public CooperativeAddressInputModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeUpdateInputModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.ToString()));
        }
    }
}