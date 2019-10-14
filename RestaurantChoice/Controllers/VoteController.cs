using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChoice.Models;
using RestaurantChoice.ViewModels;

namespace RestaurantChoice.Controllers
{
    public class VoteController : Controller
    {
        private IDal dal { get; set; }

        public VoteController() : this(new Dal())
        {
        }

        public VoteController(IDal dalIOC)
        {
            dal = dalIOC;
        }

        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfRestaurants = dal.GetAllRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NameAndPhoneNumber = string.Format("{0} ({1})", r.Name, r.PhoneNumber) }).ToList()
            };
            if (dal.AlreadyVotedByNavigator(id, Request.Browser.Browser))
                return RedirectToAction("DisplayResult", new { idSurvey = id });

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel restaurantVoteViewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(restaurantVoteViewModel);
            User user = dal.GetUserByNavigator(Request.Browser.Browser);
            if (user == null)
                return new HttpUnauthorizedResult();
            foreach (RestaurantCheckBoxViewModel restaurant in restaurantVoteViewModel.ListOfRestaurants.Where(r => r.IsCheck))
            {
                dal.AddVote(id, restaurant.Id, user.Id);
            }

            return RedirectToAction("DisplayResult", new { id = id });
        }

        public ActionResult DisplayResult(int id)
        {
            if (!dal.AlreadyVotedByNavigator(id, Request.Browser.Browser))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Results> results = dal.GetTheResults(id);
            return View(results.OrderByDescending(r => r.NumberOfVote).ToList());
        }
    }
}