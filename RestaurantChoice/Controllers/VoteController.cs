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
                return RedirectToAction("Index");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel restaurantVoteViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(restaurantVoteViewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DisplayResult()
        {
            return View();
        }
    }
}