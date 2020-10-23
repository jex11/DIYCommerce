using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.CustomProduct.Models
{
    public class CustomProductSpecViewModel
    {
        public Guid TemplateID { get; set; }
        public decimal? Breast { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hip { get; set; }
        public decimal? Sleeve { get; set; }
        public decimal? Neck { get; set; }
        public Guid TextureID { get; set; }
        public String TemplateImageBase64 { get; set; }
        public String TemplateImageType { get; set; }
        public byte[] TemplateImageByte { 
            get {
                return Convert.FromBase64String(TemplateImageBase64);
            }
        }
        
    }
}