using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RestaurantChoice.Models
{
    public class InitRestaurantChoice : DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.Restaurants.Add(new Restaurant { Id = 1, Name = "Resto pinambour", PhoneNumber = "0123445581" });
            context.Restaurants.Add(new Restaurant { Id = 2, Name = "Resto tologie", PhoneNumber = "0123434568" });
            context.Restaurants.Add(new Restaurant { Id = 5, Name = "Resto ride", PhoneNumber = "0156786798" });
            context.Restaurants.Add(new Restaurant { Id = 9, Name = "Resto toro", PhoneNumber = "0155531323" });

            base.Seed(context);
        }
    }
}