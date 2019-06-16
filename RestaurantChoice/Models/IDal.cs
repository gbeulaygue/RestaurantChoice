using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantChoice.Models
{
    public interface IDal : IDisposable
    {
        void CreateRestaurant(string name, string phoneNumber);
        void ModifyRestaurant(int id, string name, string phoneNumber);
        List<Restaurant> GetAllRestaurants();
    }
}
