using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChoice.Models;
using RestaurantChoice.ViewModels;

namespace RestaurantChoice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // ViewData : dictionary
            ViewData["message"] = "Hello from the controller";
            ViewData["date"] = DateTime.Now;
            ViewData["restaurant"] = new Restaurant { Name = "La bonne fourchette", PhoneNumber = "12345" };
            // ViewBag : dynamic
            ViewBag.message = "Hello from the controller";
            ViewBag.date = DateTime.Now;
            ViewBag.restaurant = new Restaurant { Name = "La bonne fourchette", PhoneNumber = "12345" };

            List<Restaurant> restaurantList = new List<Restaurant>()
            {
                new Restaurant { Id = 1, Name = "Resto pinambour", PhoneNumber = "1234" },
                new Restaurant { Id = 2, Name = "Resto tologie", PhoneNumber = "1234" },
                new Restaurant { Id = 5, Name = "Resto ride", PhoneNumber = "5678" },
                new Restaurant { Id = 9, Name = "Resto toro", PhoneNumber = "555" }
            };

            ViewBag.restaurantList = new SelectList(restaurantList, "Id", "Name");

            // Strongly typified view
            Restaurant restaurant = new Restaurant { Name = "L'île d'Issy", PhoneNumber = "12345" };

            return View(restaurant);
        }

        public ActionResult IndexWithViewModel()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Message = "Hello from the controller",
                Date = DateTime.Now,
                RestaurantList = new List<Restaurant> ()
                {
                    new Restaurant { Name = "Resto pinambour", PhoneNumber = "1234" },
                    new Restaurant { Name = "Resto tologie", PhoneNumber = "1234" },
                    new Restaurant { Name = "Resto ride", PhoneNumber = "5678" },
                    new Restaurant { Name = "Resto toro", PhoneNumber = "555" }
                }
            };

            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult ShowRestaurants()
        {
            List<Restaurant> listeDesRestos = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Resto pinambour", PhoneNumber = "1234" },
                new Restaurant { Id = 2, Name = "Resto tologie", PhoneNumber = "1234" },
                new Restaurant { Id = 5, Name = "Resto ride", PhoneNumber = "5678" },
                new Restaurant { Id = 9, Name = "Resto toro", PhoneNumber = "555" }
            };

            return PartialView(listeDesRestos);
        }
    }
}