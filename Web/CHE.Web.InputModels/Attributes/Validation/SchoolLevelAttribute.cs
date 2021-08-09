namespace CHE.Web.InputModels.Attributes.Validation
{
    using CHE.Data.Models;

    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataErrorMessages;

    [AttributeUsage(AttributeTargets.Property)]
    public class SchoolLevelAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var schoolLevelValue = value as string;
            ;
            if (Enum.TryParse<SchoolLevel>(schoolLevelValue, out var _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(SchoolLevelErrorMessage);
        }
    }
}