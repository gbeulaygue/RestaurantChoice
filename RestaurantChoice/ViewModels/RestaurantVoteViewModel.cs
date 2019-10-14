using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantChoice.ViewModels
{
    public class RestaurantVoteViewModel : IValidatableObject
    {
        public List<RestaurantCheckBoxViewModel> ListOfRestaurants { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ListOfRestaurants.Any(r => r.IsCheck))
                yield return new ValidationResult("You must choose at least one restaurant", new[] { "ListOfRestaurants" });
        }
    }
}