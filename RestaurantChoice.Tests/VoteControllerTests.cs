using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantChoice.Controllers;
using RestaurantChoice.Models;
using Moq;
using RestaurantChoice.ViewModels;

namespace RestaurantChoice.Tests
{
    [TestClass]
    public class VoteControllerTests
    {
        private IDal dal;
        private int idSurvey;
        private VoteController controller;

        [TestInitialize]
        public void Init()
        {
            dal = new HardDall();
            idSurvey = dal.CreateASurvey();

            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");

            controller = new VoteController(dal);
            controller.ControllerContext = controllerContext.Object;
        }

        [TestCleanup]
        public void Clean()
        {
            dal.Dispose();
        }

        [TestMethod]
        public void Index_WithSurveyNormalButWithoutUser_ReturnTheGoodViewModelAndDisplayTheView()
        {
            ViewResult view = (ViewResult)controller.Index(idSurvey);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListOfRestaurants.Count);
            Assert.AreEqual(1, viewModel.ListOfRestaurants[0].Id);
            Assert.IsFalse(viewModel.ListOfRestaurants[0].IsCheck);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListOfRestaurants[0].NameAndPhoneNumber);
        }

        [TestMethod]
        public void Index_WithSurveyNormalWithUserNotHavingVote_ReturnTheGoodViewModelAndDisplayTheView()
        {
            dal.AddUser("Nico", "1234");
            dal.AddUser("Jérémie", "1234");

            ViewResult view = (ViewResult)controller.Index(idSurvey);

            RestaurantVoteViewModel viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(3, viewModel.ListOfRestaurants.Count);
            Assert.AreEqual(1, viewModel.ListOfRestaurants[0].Id);
            Assert.IsFalse(viewModel.ListOfRestaurants[0].IsCheck);
            Assert.AreEqual("Resto pinambour (0102030405)", viewModel.ListOfRestaurants[0].NameAndPhoneNumber);
        }

        [TestMethod]
        public void Index_WithSurveyNormalButAlreadyVote_PerformedTheRedirectionToAction()
        {
            dal.AddUser("Nico", "1234");
            dal.AddUser("Jérémie", "1234");
            dal.AddVote(idSurvey, 1, 1);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controller.Index(idSurvey);

            Assert.AreEqual("DisplayResult", resultat.RouteValues["action"]);
            Assert.AreEqual(idSurvey, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void IndexPost_WithViewModelInValid_ReturnTheGoodViewModelAndDisplayTheView()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfRestaurants = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { IsCheck = false, Id = 2, NameAndPhoneNumber = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { IsCheck = false, Id = 3, NameAndPhoneNumber = "Resto toro (0102030405)"},
            }
            };
            controller.ValidatValidateTheModele(viewModel);

            ViewResult view = (ViewResult)controller.Index(viewModel, idSurvey);

            viewModel = (RestaurantVoteViewModel)view.Model;
            Assert.AreEqual(2, viewModel.ListOfRestaurants.Count);
            Assert.AreEqual(2, viewModel.ListOfRestaurants[0].Id);
            Assert.IsFalse(viewModel.ListOfRestaurants[0].IsCheck);
            Assert.AreEqual("Resto pinière (0102030405)", viewModel.ListOfRestaurants[0].NameAndPhoneNumber);
        }

        [TestMethod]
        public void IndexPost_WithViewModelValidButNoUser_ReturnAHttpUnauthorizedResult()
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfRestaurants = new List<RestaurantCheckBoxViewModel>
            {
                new RestaurantCheckBoxViewModel { IsCheck = true, Id = 2, NameAndPhoneNumber = "Resto pinière (0102030405)"},
                new RestaurantCheckBoxViewModel { IsCheck = false, Id = 3, NameAndPhoneNumber = "Resto toro (0102030405)"},
            }
            };
            controller.ValidatValidateTheModele(viewModel);

            HttpUnauthorizedResult view = (HttpUnauthorizedResult)controller.Index(viewModel, idSurvey);

            Assert.AreEqual(401, view.StatusCode);
        }

        [TestMethod]
        public void IndexPost_WithViewModelValidAndUser_CallAddVoteAndReturnGoodAction()
        {
            Mock<IDal> mock = new Mock<IDal>();
            mock.Setup(m => m.GetUser("1")).Returns(new User { Id = 1, FirstName = "Nico" });

            Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(p => p.HttpContext.Request.Browser.Browser).Returns("1");
            controller = new VoteController(mock.Object);
            controller.ControllerContext = controllerContext.Object;

            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfRestaurants = new List<RestaurantCheckBoxViewModel>
                {
                    new RestaurantCheckBoxViewModel { IsCheck = true, Id = 2, NameAndPhoneNumber = "Resto pinière (0102030405)"},
                    new RestaurantCheckBoxViewModel { IsCheck = false, Id = 3, NameAndPhoneNumber = "Resto toro (0102030405)"},
                }
            };
            controller.ValidatValidateTheModele(viewModel);

            RedirectToRouteResult resultat = (RedirectToRouteResult)controller.Index(viewModel, idSurvey);

            mock.Verify(m => m.AddVote(idSurvey, 2, 1));
            Assert.AreEqual("DisplayResult", resultat.RouteValues["action"]);
            Assert.AreEqual(idSurvey, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void DisplayResult_WhithoutGetVote_ReturnToIndex()
        {
            RedirectToRouteResult resultat = (RedirectToRouteResult)controller.DisplayResult(idSurvey);

            Assert.AreEqual("Index", resultat.RouteValues["action"]);
            Assert.AreEqual(idSurvey, resultat.RouteValues["id"]);
        }

        [TestMethod]
        public void DisplayResult_WithVote_ReturntheResults()
        {
            dal.AddUser("Nico", "1234");
            dal.AddUser("Jérémie", "1234");
            dal.AddVote(idSurvey, 1, 1);

            ViewResult view = (ViewResult)controller.DisplayResult(idSurvey);

            List<Results> model = (List<Results>)view.Model;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Resto pinambour", model[0].Name);
            Assert.AreEqual(1, model[0].NumberOfVote);
            Assert.AreEqual("0102030405", model[0].PhoneNumber);
        }
    }
}
