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
        bool ExistingRestaurant(string name);
        User GetUser(int id);
        User GetUser(string id);
        int AddUser(string firstName, string passWord);
        User Authentificate(string firstName, string passWord);
        bool AlreadyVoted(int idSurvey, string idUser);
        int CreateASurvey();
        void AddVote(int idSurvey, int idRestaurant, int idUser);
        List<Results> GetTheResults(int idSurvey);
    }
}
