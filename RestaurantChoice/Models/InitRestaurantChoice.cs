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
            context.Restaurants.Add(new Restaurant { Id = 1, Name = "Resto pinambour", PhoneNumber = "1234" });
            context.Restaurants.Add(new Restaurant { Id = 2, Name = "Resto tologie", PhoneNumber = "1234" });
            context.Restaurants.Add(new Restaurant { Id = 5, Name = "Resto ride", PhoneNumber = "5678" });
            context.Restaurants.Add(new Restaurant { Id = 9, Name = "Resto toro", PhoneNumber = "555" });

            base.Seed(context);
        }
    }
}