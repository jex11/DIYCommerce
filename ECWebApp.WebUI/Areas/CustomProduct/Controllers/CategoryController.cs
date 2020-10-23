using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.CustomProduct.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CustomProduct/Category
        public ActionResult Index()
        {
            return View();
        }
    }
}