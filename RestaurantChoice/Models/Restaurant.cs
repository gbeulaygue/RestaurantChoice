using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestaurantChoice.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The restaurant name must be filled")]
        public string Name { get; set; }
        //[Display(Name="Téléphone")]
        public string PhoneNumber { get; set; }
    }
}