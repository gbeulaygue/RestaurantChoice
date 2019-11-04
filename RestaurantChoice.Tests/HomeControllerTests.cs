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
    public class HomeControllerTests
    {
        [TestMethod]
        public void HomeController_Index_DefaultViewForwarding()
        {
            HomeController homeController = new HomeController();

            ViewResult result = (ViewResult)homeController.Index();

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void HomeController_DisplayDate_CrossreferencingIndexViewAndViewData()
        {
            HomeController homeController = new HomeController();

            ViewResult resultat = (ViewResult)homeController.DisplayDate("Nicolas");

            Assert.AreEqual("Index", resultat.ViewName);
            Assert.AreEqual(new DateTime(2012, 4, 28), resultat.ViewData["date"]);
            Assert.AreEqual("Hello Nicolas !", resultat.ViewBag.Message);
        }

        [TestMethod]
        public void HomeController_IndexPost_ReturnActionVote()
        {
            using (IDal dal = new HardDall())
            {
                HomeController controller = new HomeController(dal);

                RedirectToRouteResult result = (RedirectToRouteResult)controller.IndexPost();

                Assert.AreEqual("Index", result.RouteValues["action"]);
                Assert.AreEqual("Vote", result.RouteValues["controller"]);
                List<Results> resultats = dal.GetTheResults(1);
                Assert.IsNotNull(resultats);
            }
        }
    }
}