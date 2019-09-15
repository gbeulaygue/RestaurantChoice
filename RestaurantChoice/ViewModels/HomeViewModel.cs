using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RestaurantChoice.Models;

namespace RestaurantChoice.ViewModels
{
    public class HomeViewModel
    {
        [Display(Name ="The message")]
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public List<Restaurant> RestaurantList { get; set; }
        public string Login { get; set; }
    }
}