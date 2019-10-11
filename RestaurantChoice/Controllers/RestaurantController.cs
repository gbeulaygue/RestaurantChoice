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
        private IDal dal { get; set; }

        public RestaurantController() : this(new Dal())
        {
        }

        public RestaurantController(IDal dalIOC)
        {
            dal = dalIOC;
        }

        public ActionResult Index()
        {
            List<Restaurant> restaurants = dal.GetAllRestaurants();
            return View(restaurants);
        }

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Restaurant restaurant)
        {
            if (dal.ExistingRestaurant(restaurant.Name))
            {
                ModelState.AddModelError("Name", "This restaurant name already exists");
                return View(restaurant);
            }
            if (!ModelState.IsValid)
                return View(restaurant);
            dal.CreateRestaurant(restaurant.Name, restaurant.PhoneNumber, restaurant.Email);
            return RedirectToAction("Index");
        }

        public ActionResult ModifyRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Restaurant restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifyRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return View(restaurant);
            dal.ModifyRestaurant(restaurant.Id, restaurant.Name, restaurant.PhoneNumber);
            return RedirectToAction("Index");
        }

        // Redirection
        // code 302
        public ActionResult DisplayOpenClassRooms(string id)
        {
            return Redirect("http://fr.openclassrooms.com/");
        }

        public ActionResult ReturnHome(string id)
        {
            return RedirectToRoute(new { controller = "Accueil", action = "index" });
        }

        public ActionResult DisplaysChain()
        {
            return Content("No HTML, just a string");
        }

        public ActionResult DisplayJson()
        {
            Restaurant restaurant = new Restaurant { Id = 1, Name = "Resto pinambour" };
            return Json(restaurant, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFile()
        {
            string file = Server.MapPath("~/App_Data/MyFile.txt");
            return File(file, "application/octet-stream", "MyFile.txt");
        }

        public ActionResult LoginAuthenticates()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View();
            //return new HttpUnauthorizedResult();
            return new HttpStatusCodeResult(401);
        }
    }
}