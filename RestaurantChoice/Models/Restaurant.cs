using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestaurantChoice.Models
{
    public class Restaurant : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The restaurant name must be filled"), StringLength(80)]
        public string Name { get; set; }
        //[Display(Name="Téléphone")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "The phone number is incorrect")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) && string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult("ou must enter at least one way to contact the restaurant", new [] { "PhoneNumber", "Email" });
            }
        }
    }
}