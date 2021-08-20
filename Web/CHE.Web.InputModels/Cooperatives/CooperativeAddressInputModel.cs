namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Address;
    using static DataErrorMessages;

    public class CooperativeAddressInputModel : IMapFrom<Address>
    {
        [Required]
        [StringLength(
            CityMaxLength,
            MinimumLength = CityMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string City { get; init; }

        [Required]
        [StringLength(
            NeighbourhoodMaxLength,
            MinimumLength = NeighbourhoodMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Neighbourhood { get; init; }
    }
}