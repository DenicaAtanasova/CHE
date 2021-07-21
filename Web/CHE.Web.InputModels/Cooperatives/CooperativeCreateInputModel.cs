﻿namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Web.InputModels.Attributes.Validation;

    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Cooperative;
    using static DataErrorMessages;

    public class CooperativeCreateInputModel
    {
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
    }
}