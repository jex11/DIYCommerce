using log4net;
using ECWebApp.WebUI.App_Start;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;
using ECWebApp.WebUI.Controllers.COM;

namespace ECWebApp.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("Test");
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            log4net.Config.XmlConfigurator.Configure();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_Error(Object sender, EventArgs e)
        {

            // Code that runs when an unhandled error occurs 
            log4net.ILog log = log4net.LogManager.GetLogger("MyApp");
            if (log.IsErrorEnabled)
                log.Error("An uncaught exception occurred", this.Server.GetLastError());

        }

    }
}
