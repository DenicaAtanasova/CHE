namespace CHE.Web.InputModels.Attributes.Validation
{
    using CHE.Data.Models;

    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataErrorMessages;

    public class SchoolLevelAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var schoolLevelValue = value as string;
            ;
            if (Enum.TryParse<SchoolLevel>(schoolLevelValue, out var schoolLevel))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(SchoolLevelErrorMessage);
        }
    }
}