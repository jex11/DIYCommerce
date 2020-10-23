using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.CustomProduct.Models
{
    public class TemplateViewModel
    {
        public Guid TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateSource { get; set; }
        public string TemplateDescription { get; set; }
        public decimal? TemplatePrice { get; set; }
    }
}