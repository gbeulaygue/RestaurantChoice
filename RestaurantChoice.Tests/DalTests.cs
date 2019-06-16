using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantChoice.Models;

namespace RestaurantChoice.Tests
{
    [TestClass]
    public class DalTests
    {
        [TestInitialize]
        public void Init_BeforeEveryTest()
        {
            IDatabaseInitializer<DataBaseContext> init = new DropCreateDatabaseAlways<DataBaseContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DataBaseContext());
        }

        [TestMethod]
        public void CreateRestaurant_WithANewRestaurant_GetAllRestaurantsSendTheRestaurantBackWell()
        {
            //IDatabaseInitializer<DataBaseContext> init = new DropCreateDatabaseAlways<DataBaseContext>();
            //Database.SetInitializer(init);
            //init.InitializeDatabase(new DataBaseContext());

            using (IDal dal = new Dal())
            {
                dal.CreateRestaurant("La bonne fourchette", "0102030405");
                List<Restaurant> restaurants = dal.GetAllRestaurants();

                Assert.IsNotNull(restaurants);
                Assert.AreEqual(1, restaurants.Count);
                Assert.AreEqual("La bonne fourchette", restaurants[0].Name);
                Assert.AreEqual("0102030405", restaurants[0].PhoneNumber);
            }
        }

        [TestMethod]
        public void CreationOfANewRestaurantAndChangeOfNameAndPhoneNumber_TheChangeIsCorrectAfterReloading()
        {
            using (IDal dal = new Dal())
            {
                dal.CreateRestaurant("La bonne fourchette", "0102030405");
                List<Restaurant> restaurants = dal.GetAllRestaurants();
                int idRestaurantToChange = restaurants.First(r => r.Name == "La bonne fourchette").Id;

                dal.ModifyRestaurant(idRestaurantToChange, "La calzone", null);

                restaurants = dal.GetAllRestaurants();

                Assert.IsNotNull(restaurants);
                Assert.AreEqual(1, restaurants.Count);
                Assert.AreEqual("La calzone", restaurants[0].Name);
                Assert.IsNull(restaurants[0].PhoneNumber);
            }
        }
    }
}
