using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class ProductViewModel
    {
        public ProductInfo Product { get; set; }
        public IEnumerable<byte[]> ProductImages { get; set; }
        public IEnumerable<Color> ProductColor { get; set; }
        public int isPrimary { get; set; }
    }
}