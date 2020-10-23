using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.Admin.Models
{
    public class AccessoryCategoryInfo
    {
        public Guid AccessoriesTemplateCategoryID { get; set; }
        public string AccessoriesTemplateCategoryName { get; set; }
        public string AccessoriesTemplateCategoryDescription { get; set; }
    }
}