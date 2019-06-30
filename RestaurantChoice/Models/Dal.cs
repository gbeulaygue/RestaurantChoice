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

        public bool ExistingRestaurant(string name)
        {
            return dataBase.Restaurants.Any(r => r.Name == name);
        }

        public User GetUser(int id)
        {
            return dataBase.Users.Find(id);
        }

        public User GetUser(string id)
        {
            int idConverted;
            User foundUser = null;

            if (int.TryParse(id, out idConverted))
            {
                foundUser = GetUser(idConverted);
            }

            return foundUser;
        }

        public int AddUser(string firstName, string passWord)
        {
            dataBase.Users.Add(new User() { FirstName = firstName, PassWord = passWord });
            dataBase.SaveChanges();

            return dataBase.Users.FirstOrDefault(u => u.FirstName == firstName && u.PassWord == passWord).Id;
        }

        public User Authentificate(string firstName, string passWord)
        {
            return dataBase.Users.FirstOrDefault(u => u.FirstName == firstName && u.PassWord == passWord);
        }

        public bool AlreadyVoted(int idSurvey, string idUser)
        {
            bool alreadyVoted = false;
            Survey survey = dataBase.Surveys.Find(idSurvey);

            if(survey != null)
            {
                if (survey.Votes.Where(v => v.User.Id == GetUser(idUser).Id).Count() > 0)
                {
                    alreadyVoted = true;
                }
            }

            return alreadyVoted;
        }

        public int CreateASurvey()
        {
            dataBase.Surveys.Add(new Survey() { Date = DateTime.Now, Votes = new List<Vote>() });
            dataBase.SaveChanges();

            return dataBase.Surveys.Find(1).Id;
        }

        public void AddVote(int idSurvey, int idRestaurant, int idUser)
        {
            Restaurant restaurant = dataBase.Restaurants.Find(idRestaurant);

            dataBase.Surveys.Find(idSurvey).Votes.Add(new Vote() { Restaurant = restaurant, User = GetUser(idUser)});
            dataBase.SaveChanges();
        }

        public List<Results> GetTheResults(int idSurvey)
        {
            List<Results> results = new List<Results>();
            Survey survey = dataBase.Surveys.Find(idSurvey);
           
            foreach(Vote vote in survey.Votes.GroupBy(v => v.Restaurant.Id))
            {
                Results result = new Results();

                result.Name = vote.Restaurant.Name;
                result.PhoneNumber = vote.Restaurant.PhoneNumber;
                result.NumberOfVote = survey.Votes.Where(v => v.Restaurant.Id == vote.Restaurant.Id).Count();

                results.Add(result);
            }

            return results;
        }
    }
}