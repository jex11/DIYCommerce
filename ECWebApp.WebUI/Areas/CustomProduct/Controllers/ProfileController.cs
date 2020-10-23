using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Areas.CustomProduct.Models;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ECWebApp.WebUI.Areas.CustomProduct.Controllers
{
    public class ProfileController : Controller
    {
        private ICustomProductRepository CustomProductRepository;
        private ICartRepository CartRepository;
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;

        public ProfileController(ICustomProductRepository _CustomProductRepository, IProductRepository _ProductRepository, ICartRepository _CartRepository, ICustomerRepository _CustomerRepository)
        {
            this.CustomProductRepository = _CustomProductRepository;
            this.ProductRepository = _ProductRepository;
            this.CartRepository = _CartRepository;
            this.CustomerRepository = _CustomerRepository;
        }

        // GET: CustomProduct/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _MyInformation()
        {
            Guid CustomerID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            string CustomerName = CustomProductRepository.GetAuthorName(CustomerID);
            CustomerViewModel output = CustomerRepository.Customers
                .Where(x => x.CustomerID.Equals(CustomerID))
                .Select(x => new CustomerViewModel()
                {
                    CustomerID = CustomerID,
                    CustomerCode = x.CustomerCode,
                    CustomerFirstName = x.CustomerFirstName,
                    CustomerLastName = x.CustomerLastName,
                    CustomerAddress = x.CustomerAddress,
                    CustomerCity = x.CustomerCity,
                    CustomerState = x.CustomerState,
                    CustomerPostcode = x.CustomerPostcode,
                    CustomerCountry = x.CustomerCountry,
                    CustomerImgSource = x.CustomerImg,
                    CustomerImgType = x.CustomerImgType,
                    CustomerContact = x.CustomerContact,
                    CustomerEmail = x.CustomerEmail,
                    CustomerNRIC = x.CustomerNRIC
                    
                })
                .FirstOrDefault();
            return View(output);
        }

        public ActionResult _MySalesDesignRatio()
        {
            Guid CustomerID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            string CustomerName = CustomProductRepository.GetAuthorName(CustomerID);
            CustomerViewModel output = CustomerRepository.Customers
                .Join(CustomProductRepository.PopularAuthors, 
                        x => x.CustomerID, 
                        y=>y.DesignerID,
                        (x, y) => 
                        new CustomerViewModel()
                        {
                            CustomerID = x.CustomerID,
                            ProductCount = (int) y.ProductCount,
                            PurchasedCount = (int) y.SoldCount
                        })
                .FirstOrDefault();



            return View(output);
        }

        public ActionResult _SalesHistory()
        {
           
            return View();
        }

        public JsonResult SalesHistory()
        {
            Guid CustomerID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            string CustomerName = CustomProductRepository.GetAuthorName(CustomerID);
            List<ProductInfo> output = CustomProductRepository.CustomProductLists
                .Where(x => x.CustomerId.Equals(CustomerID))
                .Join(CartRepository.GetCartList(CustomerID),
                x => x.CustomProductId,
                y => y.CustomProductID,
                (x, y) =>
                new ProductInfo()
                {
                    ProductID = x.ProductId,
                    ProductCode = x.Product.ProductCode,
                    ProductName = x.Product.ProductName,
                    ProductQuantity = y.CartListTotalQuantity,
                    CustomProductSoldOn = y.CartListCreatedOn.ToString("MM/dd/yyyy")
                })
                .ToList();
            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _SalesAnalysis()
        {

            return View();
        }

        public JsonResult SalesAnalysis()
        {
            Guid CustomerID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            string CustomerName = CustomProductRepository.GetAuthorName(CustomerID);
            List<ProductInfo> products = CustomProductRepository.CustomProductLists
                .Where(x => x.CustomerId.Equals(CustomerID))
                .Join(CartRepository.GetCartList(CustomerID),
                x => x.CustomProductId,
                y => y.CustomProductID,
                (x, y) =>
                new ProductInfo()
                {
                    ProductQuantity = y.CartListTotalQuantity,
                    CustomProductSoldOnMonthNo = y.CartListCreatedOn.Month,
                    CustomProductSoldOnMonth = y.CartListCreatedOn.ToString("MMMM yyyy", CultureInfo.InvariantCulture)
                })
                .ToList();

            AnalysisInfo output = new AnalysisInfo(){
                labels = products.OrderBy(x => x.CustomProductSoldOnMonthNo)
                                    .Select(x => x.CustomProductSoldOnMonth)
                                    .Distinct()
                                    .ToArray()
            };
            List<decimal> datas = new List<decimal>();
            foreach(string label in output.labels){
                decimal temp = products.Where(x => x.CustomProductSoldOnMonth.Equals(label)).Select(x => x.ProductQuantity).Sum();
                datas.Add(temp);
            }

            output.datas = datas.ToArray();

            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }

    }
}