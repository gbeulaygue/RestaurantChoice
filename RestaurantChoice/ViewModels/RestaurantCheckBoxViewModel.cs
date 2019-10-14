using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantChoice.ViewModels
{
    public class RestaurantCheckBoxViewModel
    {
        public int Id { get; set; }
        public string NameAndPhoneNumber { get; set; }
        public bool IsCheck { get; set; }
    }
}