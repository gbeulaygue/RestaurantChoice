using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantChoice.Models
{
    public class Dal : IDal
    {
        private DataBaseContext dataBase;

        public Dal()
        {
            dataBase = new DataBaseContext();
        }

        public void CreateRestaurant(string name, string phoneNumber)
        {
            dataBase.Restaurants.Add(new Restaurant { Name = name, PhoneNumber = phoneNumber });
            dataBase.SaveChanges();
        }

        public void ModifyRestaurant(int id, string name, string phoneNumber)
        {
            Restaurant restaurantToModify = dataBase.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurantToModify != null)
            {
                restaurantToModify.Name         = name;
                restaurantToModify.PhoneNumber  = phoneNumber;

                dataBase.SaveChanges();
            }
        }
        public List<Restaurant> GetAllRestaurants()
        {
            return dataBase.Restaurants.ToList();
        }

        public void Dispose()
        {
            dataBase.Dispose();
        }
    }
}