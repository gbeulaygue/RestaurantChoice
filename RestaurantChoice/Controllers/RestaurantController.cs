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
        public ActionResult Index()
        {
            using (IDal dal = new Dal())
            {
                List<Restaurant> restaurants = dal.GetAllRestaurants();
                return View(restaurants);
            }
        }

        public ActionResult ModifyRestaurant(int? id)
        {
            //string id = Request.Url.AbsolutePath.Split('/').Last();
            if (id.HasValue)
            {
                ViewBag.Id = id;
                return View();
            }
            else
                return View("Error");
        }

        //public ActionResult ModifierRestaurant()
        //{
        //    string idStr = Request.QueryString["id"];
        //    int id;
        //    if (int.TryParse(idStr, out id))
        //    {
        //        ViewBag.Id = id;
        //        return View();
        //    }
        //    else
        //        return View("Error");
        //}
    }
}