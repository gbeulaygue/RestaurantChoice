using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChoice.Models;

namespace RestaurantChoice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewData["message"] = "Hello from the controller";
            ViewData["date"] = DateTime.Now;
            ViewData["restaurant"] = new Restaurant { Name = "La bonne fourchette", PhoneNumber = "12345" };

            ViewBag.message = "Hello from the controller";
            ViewBag.date = DateTime.Now;
            ViewBag.restaurant = new Restaurant { Name = "La bonne fourchette", PhoneNumber = "12345" };

            Restaurant restaurant = new Restaurant { Name = "L'île d'Issy", PhoneNumber = "12345" };

            return View(restaurant);
        }
    }
}