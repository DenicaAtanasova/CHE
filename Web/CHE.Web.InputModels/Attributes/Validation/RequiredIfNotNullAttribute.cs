namespace CHE.Web.InputModels.Attributes.Validation
{
    using System.ComponentModel.DataAnnotations;

    using static DataErrorMessages;

    public class RequiredIfNotNullAttribute : ValidationAttribute
    {
        private readonly string dependantPropertyName;

        public RequiredIfNotNullAttribute(string dependantPropertyName)
        {
            this.dependantPropertyName = dependantPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependantProperty = validationContext.ObjectType.GetProperty(this.dependantPropertyName);
            var dependantPropertyValue = (string)dependantProperty.GetValue(validationContext.ObjectInstance);
            var currentValue = (string)value;

            if (dependantPropertyValue == null)
            {
                return ValidationResult.Success;
            }
            else if(currentValue != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(RequiredIfNotNullErrorMessage, validationContext.MemberName, dependantPropertyName));
        }
    }
}