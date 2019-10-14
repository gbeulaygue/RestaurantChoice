using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantChoice.Models
{
    public interface IDal : IDisposable
    {
        void CreateRestaurant(string name, string phoneNumber, string email);
        void ModifyRestaurant(int id, string name, string phoneNumber);
        List<Restaurant> GetAllRestaurants();
        bool ExistingRestaurant(string name);
        User GetUser(int id);
        User GetUser(string idStr);
        int AddUser(string name, string passWord);
        User Authentificate(string name, string passWord);
        bool AlreadyVoted(int idSurvey, string idStr);
        int CreateASurvey();
        void AddVote(int idSurvey, int idRestaurant, int idUser);
        List<Results> GetTheResults(int idSurvey);
        bool AlreadyVotedByNavigator(int idSurvey, string idStr);
        User GetUserByNavigator(string idStr);
    }
}
