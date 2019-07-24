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
        private IDal dal;

        [TestInitialize]
        public void Init_BeforeEveryTest()
        {
            IDatabaseInitializer<DataBaseContext> init = new DropCreateDatabaseAlways<DataBaseContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DataBaseContext());

            dal = new Dal();
        }

        [TestCleanup]
        public void AfterEveryTest()
        {
            dal.Dispose();
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

        [TestMethod]
        public void CreateRestaurant_AvecUnNouveauRestaurant_ObtientTousLesRestaurantsRenvoitBienLeRestaurant()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            List<Restaurant> restos = dal.GetAllRestaurants();

            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La bonne fourchette", restos[0].Name);
            Assert.AreEqual("0102030405", restos[0].PhoneNumber);
        }

        [TestMethod]
        public void ModifierRestaurant_CreationDUnNouveauRestaurantEtChangementNameEtPhoneNumber_LaModificationEstCorrecteApresRechargement()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.ModifyRestaurant(1, "La bonne cuillère", null);

            List<Restaurant> restos = dal.GetAllRestaurants();
            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La bonne cuillère", restos[0].Name);
            Assert.IsNull(restos[0].PhoneNumber);
        }

        [TestMethod]
        public void Restaurantexist_AvecCreationDunRestauraunt_RenvoiQuilexist()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");

            bool exist = dal.ExistingRestaurant("La bonne fourchette");

            Assert.IsTrue(exist);
        }

        [TestMethod]
        public void Restaurantexist_AvecRestaurauntInexistant_RenvoiQuilexist()
        {
            bool exist = dal.ExistingRestaurant("La bonne fourchette");

            Assert.IsFalse(exist);
        }

        [TestMethod]
        public void GetUser_UserNonExistent_ReturnNull()
        {
            User user = dal.GetUser(1);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUser_IdNotNumerical_RetourneNull()
        {
            User user = dal.GetUser("abc");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void AddUser_NewUserAndRetrieving_UserIsWellRetrieved()
        {
            dal.AddUser("Nouvel user", "12345");

            User user = dal.GetUser(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel user", user.FirstName);

            user = dal.GetUser("1");

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel user", user.FirstName);
        }

        [TestMethod]
        public void Authentificate_LoginMdpOk_AuthentificationOK()
        {
            dal.AddUser("Nouvel user", "12345");

            User user = dal.Authentificate("Nouvel user", "12345");

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel user", user.FirstName);
        }

        [TestMethod]
        public void Authentificate_LoginOkMdpKo_AuthentificationKO()
        {
            dal.AddUser("Nouvel user", "12345");
            User user = dal.Authentificate("Nouvel user", "0");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authentificate_LoginKoMdpOk_AuthentificationKO()
        {
            dal.AddUser("Nouvel user", "12345");
            User user = dal.Authentificate("Nouvel", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authentificate_LoginMdpKo_AuthentificationKO()
        {
            User user = dal.Authentificate("Nouvel user", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void AlreadyVoted_WithIdNotNumerical_ReturnFalse()
        {
            bool notVoted = dal.AlreadyVoted(1, "abc");

            Assert.IsFalse(notVoted);
        }

        [TestMethod]
        public void AlreadyVoted_UserHaveNotVoted_ReturnFalse()
        {
            int idSurvey = dal.CreateASurvey();
            int iduser = dal.AddUser("Nouvel user", "12345");

            bool notVoted = dal.AlreadyVoted(idSurvey, iduser.ToString());

            Assert.IsFalse(notVoted);
        }

        [TestMethod]
        public void AlreadyVoted_UserVoted_ReturnTrue()
        {
            int idSurvey = dal.CreateASurvey();
            int iduser = dal.AddUser("Nouvel user", "12345");
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.AddVote(idSurvey, 1, iduser);

            bool voted = dal.AlreadyVoted(idSurvey, iduser.ToString());

            Assert.IsTrue(voted);
        }

        [TestMethod]
        public void GetTheResults_WithSomeChoices_ReturnWellTheResults()
        {
            int idSurvey = dal.CreateASurvey();
            int iduser1 = dal.AddUser("user1", "12345");
            int iduser2 = dal.AddUser("user2", "12345");
            int iduser3 = dal.AddUser("user3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSurvey, 1, iduser1);
            dal.AddVote(idSurvey, 3, iduser1);
            dal.AddVote(idSurvey, 4, iduser1);
            dal.AddVote(idSurvey, 1, iduser2);
            dal.AddVote(idSurvey, 1, iduser3);
            dal.AddVote(idSurvey, 3, iduser3);

            List<Results> Results = dal.GetTheResults(idSurvey);

            Assert.AreEqual(3, Results[0].NumberOfVote);
            Assert.AreEqual("Resto pinière", Results[0].Name);
            Assert.AreEqual("0102030405", Results[0].PhoneNumber);
            Assert.AreEqual(2, Results[1].NumberOfVote);
            Assert.AreEqual("Resto mate", Results[1].Name);
            Assert.AreEqual("0102030405", Results[1].PhoneNumber);
            Assert.AreEqual(1, Results[2].NumberOfVote);
            Assert.AreEqual("Resto ride", Results[2].Name);
            Assert.AreEqual("0102030405", Results[2].PhoneNumber);
        }

        [TestMethod]
        public void GetTheResults_WithTwoSurveys_GetTheGoodResults()
        {
            int idSurvey1 = dal.CreateASurvey();
            int iduser1 = dal.AddUser("user1", "12345");
            int iduser2 = dal.AddUser("user2", "12345");
            int iduser3 = dal.AddUser("user3", "12345");
            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");
            dal.AddVote(idSurvey1, 1, iduser1);
            dal.AddVote(idSurvey1, 3, iduser1);
            dal.AddVote(idSurvey1, 4, iduser1);
            dal.AddVote(idSurvey1, 1, iduser2);
            dal.AddVote(idSurvey1, 1, iduser3);
            dal.AddVote(idSurvey1, 3, iduser3);

            int idSurvey2 = dal.CreateASurvey();
            dal.AddVote(idSurvey2, 2, iduser1);
            dal.AddVote(idSurvey2, 3, iduser1);
            dal.AddVote(idSurvey2, 1, iduser2);
            dal.AddVote(idSurvey2, 4, iduser3);
            dal.AddVote(idSurvey2, 3, iduser3);

            List<Results> Results1 = dal.GetTheResults(idSurvey1);
            List<Results> Results2 = dal.GetTheResults(idSurvey2);

            Assert.AreEqual(3, Results1[0].NumberOfVote);
            Assert.AreEqual("Resto pinière", Results1[0].Name);
            Assert.AreEqual("0102030405", Results1[0].PhoneNumber);
            Assert.AreEqual(2, Results1[1].NumberOfVote);
            Assert.AreEqual("Resto mate", Results1[1].Name);
            Assert.AreEqual("0102030405", Results1[1].PhoneNumber);
            Assert.AreEqual(1, Results1[2].NumberOfVote);
            Assert.AreEqual("Resto ride", Results1[2].Name);
            Assert.AreEqual("0102030405", Results1[2].PhoneNumber);

            Assert.AreEqual(1, Results2[0].NumberOfVote);
            Assert.AreEqual("Resto pinambour", Results2[0].Name);
            Assert.AreEqual("0102030405", Results2[0].PhoneNumber);
            Assert.AreEqual(2, Results2[1].NumberOfVote);
            Assert.AreEqual("Resto mate", Results2[1].Name);
            Assert.AreEqual("0102030405", Results2[1].PhoneNumber);
            Assert.AreEqual(1, Results2[2].NumberOfVote);
            Assert.AreEqual("Resto pinière", Results2[2].Name);
            Assert.AreEqual("0102030405", Results2[2].PhoneNumber);
        }
    }
}
