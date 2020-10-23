using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.CustomProduct.Models
{
    public class AccessoryInfo
    {
        public System.Guid AccessoriesID { get; set; }
        public System.Guid AccessoriesTemplateID { get; set; }
        public System.Guid ProductID { get; set; }
        public decimal AccessoriesX { get; set; }
        public decimal AccessoriesY { get; set; }
    }
}