using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantChoice.Models;

namespace RestaurantChoice.Tests
{
    public class HardDall : IDal
    {
        private List<Restaurant> listOfTheRestaurants;
        private List<User> listOfTheUsers;
        private List<Survey> listOfTheSurveys;

        public HardDall()
        {
            listOfTheRestaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, Name = "Resto pinambour", PhoneNumber = "0102030405", Email = "LaBonneFourchette@email.com"},
            new Restaurant { Id = 2, Name = "Resto pinière", PhoneNumber = "0102030405", Email = "RestaurantPinière@email.com"},
            new Restaurant { Id = 3, Name = "Resto toro", PhoneNumber = "0102030405", Email = "RestaurantToro@email.com"},
        };
            listOfTheUsers = new List<User>();
            listOfTheSurveys = new List<Survey>();
        }

        public List<Restaurant> GetAllRestaurants()
        {
            return listOfTheRestaurants;
        }

        public void CreateRestaurant(string Name, string PhoneNumber, string Email)
        {
            int id = listOfTheRestaurants.Count == 0 ? 1 : listOfTheRestaurants.Max(r => r.Id) + 1;
            listOfTheRestaurants.Add(new Restaurant { Id = id, Name = Name, PhoneNumber = PhoneNumber, Email = Email });
        }

        public void ModifyRestaurant(int id, string Name, string PhoneNumber)
        {
            Restaurant Restaurant = listOfTheRestaurants.FirstOrDefault(r => r.Id == id);
            if (Restaurant != null)
            {
                Restaurant.Name = Name;
                Restaurant.PhoneNumber = PhoneNumber;
            }
        }

        public bool ExistingRestaurant(string Name)
        {
            return listOfTheRestaurants.Any(Restaurant => string.Compare(Restaurant.Name, Name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string Name, string motDePasse)
        {
            int id = listOfTheUsers.Count == 0 ? 1 : listOfTheUsers.Max(u => u.Id) + 1;
            listOfTheUsers.Add(new User { Id = id, FirstName = Name, PassWord = motDePasse });
            return id;
        }

        public User Authentificate(string Name, string motDePasse)
        {
            return listOfTheUsers.FirstOrDefault(u => u.FirstName == Name && u.PassWord == motDePasse);
        }

        public User GetUser(int id)
        {
            return listOfTheUsers.FirstOrDefault(u => u.Id == id);
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
            int id = listOfTheSurveys.Count == 0 ? 1 : listOfTheSurveys.Max(s => s.Id) + 1;
            listOfTheSurveys.Add(new Survey { Id = id, Date = DateTime.Now, Votes = new List<Vote>() });
            return id;
        }

        public void AddVote(int idSurvey, int idRestaurant, int idUser)
        {
            Vote vote = new Vote
            {
                Restaurant = listOfTheRestaurants.First(r => r.Id == idRestaurant),
                User = listOfTheUsers.First(u => u.Id == idUser)
            };
            Survey Survey = listOfTheSurveys.First(s => s.Id == idSurvey);
            Survey.Votes.Add(vote);
        }

        public bool AlreadyVoted(int idSurvey, string idStr)
        {
            User User = GetUser(idStr);
            if (User == null)
                return false;
            Survey Survey = listOfTheSurveys.First(s => s.Id == idSurvey);
            return Survey.Votes.Any(v => v.User.Id == User.Id);
        }

        public List<Results> GetTheResults(int idSurvey)
        {
            List<Restaurant> restaurants = GetAllRestaurants();
            List<Results> Results = new List<Results>();
            Survey Survey = listOfTheSurveys.First(s => s.Id == idSurvey);
            foreach (IGrouping<int, Vote> grouping in Survey.Votes.GroupBy(v => v.Restaurant.Id))
            {
                int idRestaurant = grouping.Key;
                Restaurant Restaurant = restaurants.First(r => r.Id == idRestaurant);
                int NamebreDeVotes = grouping.Count();
                Results.Add(new Results { Name = Restaurant.Name, PhoneNumber = Restaurant.PhoneNumber, NumberOfVote = NamebreDeVotes });
            }
            return Results;
        }

        public void Dispose()
        {
            listOfTheRestaurants = new List<Restaurant>();
            listOfTheUsers = new List<User>();
            listOfTheSurveys = new List<Survey>();
        }
    }
}