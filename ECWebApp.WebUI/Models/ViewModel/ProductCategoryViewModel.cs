using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        
    }
}