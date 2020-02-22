namespace CHE.Web.InputModels.Attributes.Validation
{
    using CHE.Services.Data;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class GradeAttribute : ValidationAttribute
    {
        private const string ERROR_MESSAGE = "Grade must be between first and eighth.";

        public string GetErrorMessage() => ERROR_MESSAGE;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gradesService = (IGradesService)validationContext.GetService(typeof(IGradesService));
            var grades = gradesService.GetAllAsync().GetAwaiter().GetResult();
            var gradeValue = value as string;
            if (grades.Any(x => x == gradeValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}