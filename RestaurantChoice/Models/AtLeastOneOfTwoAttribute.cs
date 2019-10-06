using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RestaurantChoice.Models
{
    public class AtLeastOneOfTwoAttribute : ValidationAttribute, IClientValidatable
    {
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }

        public AtLeastOneOfTwoAttribute() : base("You must enter at least one way to contact the restaurant")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo[] properties = validationContext.ObjectType.GetProperties();
            PropertyInfo info1 = properties.FirstOrDefault(p => p.Name == Parameter1);
            PropertyInfo info2 = properties.FirstOrDefault(p => p.Name == Parameter2);

            string value1 = info1.GetValue(validationContext.ObjectInstance) as string;
            string value2 = info2.GetValue(validationContext.ObjectInstance) as string;

            if (string.IsNullOrWhiteSpace(value1) && string.IsNullOrWhiteSpace(value2))
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "verifcontact";
            rule.ErrorMessage = ErrorMessage;
            rule.ValidationParameters.Add("parameter1", Parameter1);
            rule.ValidationParameters.Add("parameter2", Parameter2);

            return new List<ModelClientValidationRule> { rule };
        }
    }
}