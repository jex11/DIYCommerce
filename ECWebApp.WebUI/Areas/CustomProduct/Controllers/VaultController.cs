using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECWebApp.WebUI.Areas.CustomProduct.Controllers
{
    public class VaultController : Controller
    {
        private ICustomProductRepository CustomProductRepository;
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;

        public VaultController(ICustomProductRepository _CustomProductRepository, IProductRepository _ProductRepository, ICustomerRepository _CustomerRepository)
        {
            this.CustomProductRepository = _CustomProductRepository;
            this.ProductRepository = _ProductRepository;
            this.CustomerRepository = _CustomerRepository;
        }

        // GET: CustomProduct/Vault
        public ActionResult Index()
        {
            Guid AuthorID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            string AuthorName = CustomProductRepository.GetAuthorName(AuthorID);
            List<Guid> OwnedProducts = CustomProductRepository.OwnedCustomProducts(AuthorID).ToList();
            List<ProductInfo> output = CustomProductRepository.CustomProducts
                                                        .Where(x => OwnedProducts.Contains(x.ProductId))
                                                        .OrderByDescending(x => x.ProductCreatedOn)
                                                        .ThenBy(x => x.ProductName)
                                                        .Take(20)
                                                        .Select(x => new ProductInfo {
                                                            ProductID = x.ProductId,
                                                            ProductCode = x.ProductCode,
                                                            ProductName = x.ProductName,
                                                            ProductImageByte = x.Images.Select(y => y.ProductImageSource).FirstOrDefault(),
                                                            ProductImageType = x.Images.Select(y => y.ProductImageType).FirstOrDefault(),
                                                            ProductRetailPrice = x.ProductRetailPrice,
                                                            CustomProductCreatedOn = x.ProductCreatedOn.ToString("MM/dd/yyyy"),
                                                            CustomProductAuthorID = AuthorID,
                                                            CustomProductAuthorName = AuthorName
                                                        })
                                                        .ToList();
            
            return View(output);
        }

        public void DeleteOwnedProducts(Guid id)
        {
            ProductRepository.DeleteProduct(id);

        }
    }
}