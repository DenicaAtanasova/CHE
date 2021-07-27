namespace CHE.Web.InputModels.Attributes.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataErrorMessages;

    [AttributeUsage(AttributeTargets.Property)]
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string previousDate;

        public DateAfterAttribute(string previousDate)
        {
            this.previousDate = previousDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(this.previousDate);
            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
            var currentValue = (DateTime)value;
            if (comparisonValue < currentValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(DateAfterErrorMessage, previousDate));
        }
    }
}