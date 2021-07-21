namespace CHE.Web.InputModels.Attributes.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using static DataErrorMessages;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class GradeAttribute : ValidationAttribute
    {
        private readonly string[] VALID_GRADES = 
        {
            "First",
            "Second",
            "Third",
            "Forth",
            "Fifth",
            "Sixth",
            "Seventh",
            "Eighth"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gradeValue = value as string;
            if (VALID_GRADES.Any(x => x == gradeValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GradeErroMessage);
        }
    }
}