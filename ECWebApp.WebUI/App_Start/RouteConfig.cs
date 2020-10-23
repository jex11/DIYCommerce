using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ECWebApp.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Home",
               "",
               new
               {
                   Controller = "Home",
                   action = "Home"
               }
           );

            routes.MapRoute(
               "Member",
               "Member/{action}/{ReturnUrl}",
               new
               {
                   Controller = "Home",
                   ReturnUrl = UrlParameter.Optional
               }
           );



            #region Product
            routes.MapRoute(
             "Product",
             "Product/{category}/{page}",
             new
             {
                 Controller = "Product",
                 action = "List"
                 //category = (int)1,
                 //page = 1
             },
             new
             {
                 page = @"\d+"
             });
            #endregion

            #region Customer
            routes.MapRoute(
              "Customer",
              "Customer",
              new
              {
                  Controller = "Customer",
                  action = "CustomerPage"
              });

            routes.MapRoute(
              "Customer_Tab",
              "Customer/{action}/{CustomerID}/{AddressID}",
              new
              {
                  Controller = "Customer",
                  AddressID = UrlParameter.Optional
              });

            routes.MapRoute(
             "Customer_Verification_Account",
             "{Controller}/{action}/{CustomerID}/{token}",
             new
             {
                 Controller = "Customer",
                 action="ConfirmEmail"
                 
             });
            
            #endregion

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
