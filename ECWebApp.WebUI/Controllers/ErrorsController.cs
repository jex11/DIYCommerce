using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Controllers.COM
{
    public class ErrorsController : Controller
    {
       public ActionResult Error_404()
        {
            
            return View();
        }

       public ActionResult Error_505() 
       {
           return View();       
       }
    }
}