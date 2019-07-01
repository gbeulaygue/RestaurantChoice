using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantChoice.Models
{
    public class Dal : IDal
    {
        private DataBaseContext dataBase;

        public Dal()
        {
            dataBase = new DataBaseContext();
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return dataBase.Restaurants.ToList();
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

        public bool ExistingRestaurant(string name)
        {
            return dataBase.Restaurants.Any(r => string.Compare(r.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string firstName, string passWord)
        {
            string passWordEncode = EncodeMD5(passWord);
            User user = new User { FirstName = firstName, PassWord = passWordEncode };

            dataBase.Users.Add(user);
            dataBase.SaveChanges();

            return user.Id;
        }

        public User Authentificate(string firstName, string passWord)
        {
            string passWordEncode = EncodeMD5(passWord);
            return dataBase.Users.FirstOrDefault(u => u.FirstName == firstName && u.PassWord == passWordEncode);
        }

        public User GetUser(int id)
        {
            return dataBase.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
                return GetUser(id);
            return null;
        }

        public int CreateASurvey()
        {
            Survey survey = new Survey { Date = DateTime.Now };
            dataBase.Surveys.Add(survey);
            dataBase.SaveChanges();

            return survey.Id;
        }

        public void AddVote(int idSurvey, int idRestaurant, int idUser)
        {
            Vote vote = new Vote
            {
                Restaurant = dataBase.Restaurants.First(r => r.Id == idRestaurant),
                User = GetUser(idUser)
            };
            Survey survey = dataBase.Surveys.First(s => s.Id == idSurvey);
            if(survey.Votes == null)
                survey.Votes = new List<Vote>();
            survey.Votes.Add(vote);
            dataBase.SaveChanges();
        }

        public List<Results> GetTheResults(int idSurvey)
        {
            List<Restaurant> restaurants = GetAllRestaurants();
            List<Results> results = new List<Results>();
            Survey survey = dataBase.Surveys.First(s => s.Id == idSurvey);

            foreach (IGrouping<int, Vote> grouping in survey.Votes.GroupBy(v => v.Restaurant.Id))
            {
                int idRestaurant = grouping.Key;
                Restaurant restaurant = dataBase.Restaurants.First(r => r.Id == idRestaurant);
                int numberOfVotes = grouping.Count();
                results.Add(new Results { Name = restaurant.Name, PhoneNumber = restaurant.PhoneNumber, NumberOfVote = numberOfVotes});
            }

            return results;
        }

        public bool AlreadyVoted(int idSurvey, string idStr)
        {
            int id;
            if(int.TryParse(idStr, out id))
            {
                Survey survey = dataBase.Surveys.First(s => s.Id == idSurvey);
                if (survey.Votes == null)
                    return false;
                return survey.Votes.Any(v => v.User != null && v.User.Id == id);
            }
            return false;
        }

        public void Dispose()
        {
            dataBase.Dispose();
        }

        private string EncodeMD5(string passWord)
        {
            string passWordToEncode = "RestaurantChoice" + passWord + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passWordToEncode)));
        }
    }
}