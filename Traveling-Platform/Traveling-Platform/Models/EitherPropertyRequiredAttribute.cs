namespace Traveling_Platform.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class EitherPropertyRequiredAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public EitherPropertyRequiredAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (value == null && otherPropertyValue == null)
            {
                return new ValidationResult($"At least one of the fields '{validationContext.MemberName}' or '{_otherPropertyName}' is required.");
            }

            return ValidationResult.Success;
        }
    }
}
