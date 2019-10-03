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

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Restaurant restaurant)
        {
            using (IDal dal = new Dal())
            {
                if (dal.ExistingRestaurant(restaurant.Name))
                {
                    ModelState.AddModelError("Name", "This restaurant name already exists");
                    return View(restaurant);
                }
                if (!ModelState.IsValid)
                    return View(restaurant);
                dal.CreateRestaurant(restaurant.Name, restaurant.PhoneNumber);
                return RedirectToAction("Index");
            }
        }

        public ActionResult ModifyRestaurant(int? id)
        {
            if (id.HasValue)
            {
                using (IDal dal = new Dal())
                {
                    Restaurant restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
                    if (restaurant == null)
                        return View("Error");
                    return View(restaurant);
                }
            }
            else
                return View("Error");
        }

        [HttpPost]
        public ActionResult ModifyRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return View(restaurant);
            using (IDal dal = new Dal())
            {
                dal.ModifyRestaurant(restaurant.Id, restaurant.Name, restaurant.PhoneNumber);
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult ModifyRestaurant(Restaurant restaurant)
        //{
        //    if (string.IsNullOrWhiteSpace(restaurant.Name))
        //    {
        //        ViewBag.ErrorMessage = "The restaurant name must be filled";
        //        return View(restaurant);
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.ErrorMessage = ModelState["Name"].Errors[0].ErrorMessage;
        //        return View(restaurant);
        //    }
        //}

        //[HttpPost]
        //public ActionResult ModifyRestaurant(int? id, string name, string phoneNumber)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            dal.ModifyRestaurant(id.Value, name, phoneNumber);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    else
        //        return View("Error");
        //}

        //public ActionResult ModifyRestaurant(int? id, string name, string phoneNumber)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            if (Request.HttpMethod == "POST")
        //            {
        //                dal.ModifyRestaurant(id.Value, name, phoneNumber);
        //            }

        //            Restaurant restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
        //            if (restaurant == null)
        //                return View("Error");
        //            return View(restaurant);
        //        }
        //    }
        //    else
        //        return View("Error");
        //}

        //public ActionResult ModifyRestaurant(int? id)
        //{
        //    if (id.HasValue)
        //    {
        //        using (IDal dal = new Dal())
        //        {
        //            if (Request.HttpMethod == "POST")
        //            {
        //                string newName = Request.Form["Name"];
        //                string newPhoneNumber = Request.Form["PhoneNumber"];
        //                dal.ModifyRestaurant(id.Value, newName, newPhoneNumber);
        //            }

        //            Restaurant restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
        //            if (restaurant == null)
        //                return View("Error");
        //            return View(restaurant);
        //        }
        //    }
        //    else
        //        return View("Error");
        //}

        //public ActionResult ModifyRestaurant(int? id)
        //{
        //    //string id = Request.Url.AbsolutePath.Split('/').Last();
        //    if (id.HasValue)
        //    {
        //        ViewBag.Id = id;
        //        return View();
        //    }
        //    else
        //        return View("Error");
        //}

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