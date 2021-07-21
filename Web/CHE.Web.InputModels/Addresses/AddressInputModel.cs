namespace CHE.Web.InputModels.Addresses
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class AddressInputModel : IMapFrom<Address>
    {
        [Required]
        public string City { get; init; }

        [Required]
        public string Neighbourhood { get; init; }
    }
}