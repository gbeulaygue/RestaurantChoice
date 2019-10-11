using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantChoice.Controllers;
using RestaurantChoice.Models;

namespace RestaurantChoice.Tests
{
    [TestClass]
    public class RestaurantControllerTests
    {
        [TestMethod]
        public void RestaurantController_Index_TheControleurIsOk()
        {
            using (IDal dal = new HardDall())
            {
                RestaurantController controller = new RestaurantController(dal);

                ViewResult resultat = (ViewResult)controller.Index();

                List<Restaurant> model = (List<Restaurant>)resultat.Model;
                Assert.AreEqual("Resto pinambour", model[0].Name);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifyRestaurantWithInvalidRestaurant_ReturnDefaultView()
        {
            using (IDal dal = new HardDall())
            {
                RestaurantController controller = new RestaurantController(dal);
                controller.ModelState.AddModelError("Name", "The name of the restaurant must be entered");

                ViewResult resultat = (ViewResult)controller.ModifyRestaurant(new Restaurant { Id = 1, Name = null, PhoneNumber = "0102030405" });

                Assert.AreEqual(string.Empty, resultat.ViewName);
                Assert.IsFalse(resultat.ViewData.ModelState.IsValid);
            }
        }

        [TestMethod]
        public void RestaurantController_ModifyRestaurantWithInvalidRestaurantAndBindingModel_ReturnDefaultView()
        {
            RestaurantController controller = new RestaurantController(new HardDall());
            Restaurant restaurant = new Restaurant { Id = 1, Name = null, PhoneNumber = "0102030405" };
            controller.ValidatValidateTheModele(restaurant);

            ViewResult result = (ViewResult)controller.ModifyRestaurant(restaurant);

            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void RestaurantController_ModifyRestaurantWithValidRestaurant_CreateRestaurantAndReturnIndexView()
        {
            using (IDal dal = new HardDall())
            {
                RestaurantController controller = new RestaurantController(dal);
                Restaurant resto = new Restaurant { Id = 1, Name = "Resto mate", PhoneNumber = "0102030405" };
                controller.ValidatValidateTheModele(resto);

                RedirectToRouteResult resultat = (RedirectToRouteResult)controller.ModifyRestaurant(resto);

                Assert.AreEqual("Index", resultat.RouteValues["action"]);
                Restaurant foundResaurant = dal.GetAllRestaurants().First();
                Assert.AreEqual("Resto mate", foundResaurant.Name);
            }
        }
    }
}
