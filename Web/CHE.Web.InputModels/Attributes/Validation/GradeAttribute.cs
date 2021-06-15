namespace CHE.Web.InputModels.Attributes.Validation
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class GradeAttribute : ValidationAttribute
    {
        private const string ERROR_MESSAGE = "Grade must be between first and eighth.";
        private readonly string[] VALID_GRADES = 
        {
            "First",
            "Second",
            "Third",
            "Forth",
            "Fifth",
            "Sixth",
            "Seventh",
            "Eight"
        };

        public string GetErrorMessage() => ERROR_MESSAGE;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gradeValue = value as string;
            if (VALID_GRADES.Any(x => x == gradeValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}