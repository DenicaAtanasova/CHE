namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class CooperativeAddressInputModel : IMapFrom<Address>
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Neighbourhood { get; set; }
    }
}