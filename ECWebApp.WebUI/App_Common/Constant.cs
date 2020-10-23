using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.App_Common
{
    public class Constant
    {
        public const int PRODUCT_PAGE_SIZE = 5;
        public static readonly Guid CUSTOMER_CATEGORY = Guid.Parse(ConfigurationManager.AppSettings["CustomerDefaultClass"]);
        public static readonly int CUSTOM_PRODUCT_CATEGORY= Int32.Parse(ConfigurationManager.AppSettings["CustomProductCategory"]);
        public static readonly Guid CUSTOM_PRODUCT_FOLDER = Guid.Parse(ConfigurationManager.AppSettings["CustomProductFolder"]);

        //Social Network Link
        public static string FACEBOOK_LINK = ConfigurationManager.AppSettings["FacebookLink"];
        public static string TWITTER_LINK = ConfigurationManager.AppSettings["TwitterLink"];
        public static string INSTAGRAM_LINK = ConfigurationManager.AppSettings["InstagramLink"];

        //Google Map
        public static string GOOGLE_API_KEY = ConfigurationManager.AppSettings["GoogleApiKey"];
        public static readonly double GOOGLE_MAP_LAT = double.Parse(ConfigurationManager.AppSettings["GoogleMapLat"]);
        public static readonly double GOOGLE_MAP_LNG = double.Parse(ConfigurationManager.AppSettings["GoogleMapLng"]);

        //Company Info
        public static string COMPANY_NAME = ConfigurationManager.AppSettings["CompanyName"];
        public static string COMPANY_ADDRESS = ConfigurationManager.AppSettings["CompanyAddress"];
        public static string COMPANY_CONTACT = ConfigurationManager.AppSettings["CompanyContact"];
        public static string COMPANY_PERSON_IN_CHARGE = ConfigurationManager.AppSettings["CompanyPersonInCharge"];
    }
}