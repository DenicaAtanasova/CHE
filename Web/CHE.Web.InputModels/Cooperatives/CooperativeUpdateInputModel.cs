namespace CHE.Web.InputModels.Cooperatives
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Addresses;
    using CHE.Web.InputModels.Attributes.Validation;

    using System.ComponentModel.DataAnnotations;

    public class CooperativeUpdateInputModel : IMapExplicitly
    {
        public string Id { get; init; }

        [Required]
        [StringLength(20)]
        public string Name { get; init; }

        [Required]
        public string Info { get; init; }

        [Required]
        [Grade]
        public string Grade { get; init; }

        public AddressInputModel Address { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeUpdateInputModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));
        }
    }
}