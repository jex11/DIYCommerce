using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECWebApp.Domain;
using System.Web.Mvc;

namespace ECWebApp.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Folder> Folders { get; }
        IEnumerable<Color> Colors { get; }
        IEnumerable<Image> Images { get; }
        IEnumerable<Category> Categories { get; }
        IEnumerable<SelectListItem> ScaleList { get; }
        
        //Products
        decimal? GetProductPrice(Guid ProductID);
        void AddProduct(Product product, Image primaryImage);
        void UpdateProduct(Product product, Image primaryImage);
        void DeleteProduct(Guid ProductID);
        List<vw_ProductList> GetProducts(Guid CategoryID, int PageIndex, int PageSize, string OrderBy);

        //Folders
        void AddFolder(Folder folder);
        void UpdateFolder(Folder folder);
        void DeleteFolder(Guid FolderID);
        void SearchFolder(Folder folder);

        //Categories
        void AddCategory(Category category);
        

        //Images
        List<Image> GetPrimaryImages(Guid location);
    }
}
