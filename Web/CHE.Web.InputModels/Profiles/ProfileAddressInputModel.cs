﻿namespace CHE.Web.InputModels.Profiles
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Address;
    using static DataErrorMessages;

    public class ProfileAddressInputModel : IMapFrom<Address>
    {
        [StringLength(
            CityMaxLength,
            MinimumLength = CityMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string City { get; set; }

        [StringLength(
            NeighbourhoodMaxLength,
            MinimumLength = NeighbourhoodMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Neighbourhood { get; set; }
    }
}