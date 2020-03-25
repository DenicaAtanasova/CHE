namespace CHE.Web.InputModels.Attributes.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateAfterAttribute : ValidationAttribute
    {
        private const string ERROR_MESSAGE = "Date must be after {0}.";

        private readonly string _previousDate;

        public DateAfterAttribute(string previousDate)
        {
            this._previousDate = previousDate;
        }

        public string GetErrorMessage() => string.Format(ERROR_MESSAGE, this._previousDate);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(this._previousDate);
            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
            var currentValue = (DateTime)value;
            if (comparisonValue < currentValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}
