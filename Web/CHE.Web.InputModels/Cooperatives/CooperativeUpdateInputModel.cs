namespace CHE.Web.InputModels.Cooperatives
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Attributes.Validation;

    public class CooperativeUpdateInputModel : IMapExplicitly
    {
        public string Id { get; set; }

        public string CreatorId { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        public string Info { get; set; }

        [Required]
        [Grade]
        public string Grade { get; set; }

        public CooperativeAddressInputModel Address { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cooperative, CooperativeUpdateInputModel>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.Value));
        }
    }
}