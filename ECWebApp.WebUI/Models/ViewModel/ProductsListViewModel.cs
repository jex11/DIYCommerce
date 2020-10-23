using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductInfo> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<FolderInfo> FolderInfo { get; set; }
        public Guid FolderFrom { get; set; }
        public Guid FolderLocation { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int? CurrentCategory { get; set; }
    }
}