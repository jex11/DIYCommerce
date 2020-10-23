using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.CustomProduct
{
    public class CustomProductAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CustomProduct";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CustomProduct_default",
                "CustomProduct",
                new 
                { 
                    controller= "Custom",
                    action = "Index",
                    id = UrlParameter.Optional 
                }
                );
                context.MapRoute(
                "CustomProduct",
                "CustomProduct/{controller}/{action}/{id}",
                new 
                { 
                    id = UrlParameter.Optional 
                }
                );
            
        }
    }
}