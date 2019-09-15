using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using RestaurantChoice.Models;

namespace RestaurantChoice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IDatabaseInitializer<DataBaseContext> init = new InitRestaurantChoice();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DataBaseContext());
        }
    }
}
