using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ECWebApp.WebUI.Areas.CustomProduct.Controllers
{
    public class GalleryController : Controller
    {
        private ICustomProductRepository CustomProductRepository;
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;
        private ICartRepository CartRepository;

        public GalleryController(ICustomProductRepository _CustomProductRepository, IProductRepository _ProductRepository, ICustomerRepository _CustomerRepository, ICartRepository _CartRepository)
        {
            this.CustomProductRepository = _CustomProductRepository;
            this.ProductRepository = _ProductRepository;
            this.CustomerRepository = _CustomerRepository;
            this.CartRepository = _CartRepository;
        }

        // GET: CustomProduct/Gallery
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// GET: Return Most recent Custom Products
        /// </summary>
        /// <returns></returns>
        public ActionResult _MostRecent()
        {
            List<ProductInfo> output = CustomProductRepository.CustomProducts
                .OrderByDescending(x => x.ProductCreatedOn)
                .ThenBy(x => x.ProductName)
                .Take(6)
                .Select(x => new ProductInfo
                {
                    ProductID = x.ProductId,
                    ProductName = x.ProductName,
                    ProductRetailPrice = x.ProductRetailPrice,
                    ProductImageByte = x.Images.Select(y => y.ProductImageSource).FirstOrDefault(),
                    ProductImageType = x.Images.Select(y => y.ProductImageType).FirstOrDefault(),
                    CustomProductCreatedOn = x.ProductCreatedOn.ToString("MM/dd/yyyy"),
                    CustomProductAuthorName = CustomProductRepository.PopularProducts.Where(y => y.ProductId.Equals(x.ProductId)).Select(y => y.DesignerFirstName + " " + y.DesignerLastName).FirstOrDefault()
                })
                .ToList();
            
            return View(output);
        }

        /// <summary>
        /// GET: Return Most Popular Custom Products
        /// </summary>
        /// <returns></returns>
        public ActionResult _MostPopular()
        {

            List<ECWebApp.Domain.vw_PopularProduct> CustomProducts = CustomProductRepository.PopularProducts.OrderByDescending(x => x.SoldCount).Take(6).ToList();
            List<Guid> ProductIds = CustomProducts.Select(x => x.ProductId).ToList();
            List<ProductInfo> output = CustomProductRepository.CustomProducts.Where(x => ProductIds.Contains(x.ProductId))
                .Select(x => new ProductInfo{
                    ProductID = x.ProductId,
                    ProductName = x.ProductName,
                    ProductRetailPrice = x.ProductRetailPrice,
                    CustomProductCreatedOn = x.ProductCreatedOn.ToString("MM/dd/yyyy"),
                    ProductImageByte = x.Images.Select(y => y.ProductImageSource).FirstOrDefault(),
                    ProductImageType = x.Images.Select(y => y.ProductImageType).FirstOrDefault(),
                    CustomProductAuthorName = CustomProducts.Where(y => y.ProductId.Equals(x.ProductId)).Select(y => y.DesignerFirstName + " " + y.DesignerLastName).FirstOrDefault(),
                    PurchasedCount = CustomProducts.Where(y => y.ProductId.Equals(x.ProductId)).Select(y => y.SoldCount).FirstOrDefault()
                }).ToList();

            return PartialView(output);
        }

        public ActionResult _PopularAuthor()
        {
            List<ECWebApp.Domain.vw_PopularAuthor> Designers = CustomProductRepository.PopularAuthors.OrderByDescending(x => x.SoldCount).Take(6).ToList();
            List<Guid> DesignerIds = Designers.Select(x => x.DesignerID).ToList();
            List<CustomerViewModel> output = CustomerRepository.Customers.Where(x => DesignerIds.Contains(x.CustomerID))
                .Select(x => new CustomerViewModel{
                    CustomerID = x.CustomerID,
                    CustomerFirstName = x.CustomerFirstName,
                    CustomerLastName = x.CustomerLastName,
                    CustomerImgSource = x.CustomerImg,
                    CustomerImgType = x.CustomerImgType,
                    ProductCount = (int)Designers.Where(y => y.DesignerID == x.CustomerID).Select(y => y.ProductCount).FirstOrDefault(),
                    PurchasedCount = (int)Designers.Where(y => y.DesignerID == x.CustomerID).Select(y => y.SoldCount).FirstOrDefault()
                }).ToList();

            return PartialView(output);
        }
        #region Api Method
        
        #endregion
    }
}