using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChoice.Models;

namespace RestaurantChoice.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            using (IDal dal = new Dal())
            {
                List<Restaurant> restaurants = dal.GetAllRestaurants();
                return View(restaurants);
            }
        }
    }
}