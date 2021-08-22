namespace CHE.Web.InputModels.Attributes.Validation
{
    using CHE.Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataErrorMessages;

    [AttributeUsage(AttributeTargets.Property)]
    public class GradeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gradeValue = value as string;
            ;
            if (Enum.TryParse<Grade>(gradeValue, out var _))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GradeErroMessage);
        }
    }
}