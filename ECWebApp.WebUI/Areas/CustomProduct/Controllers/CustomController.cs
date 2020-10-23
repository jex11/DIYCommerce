using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using ECWebApp.Domain;
using ECWebApp.WebUI.Areas.CustomProduct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECWebApp.WebUI.Models;
using System.Web.Security;
using ECWebApp.WebUI.App_Common;
using ECWebApp.WebUI.Areas.Admin.Models;
using System.Web.Script.Serialization;

namespace ECWebApp.WebUI.Areas.CustomProduct.Controllers
{
    public class CustomController : Controller
    {
        private ICustomProductRepository CustomProductRepository;
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;
        private ICartRepository CartRepository;

        public CustomController(ICustomProductRepository _CustomProductRepository, IProductRepository _ProductRepository, ICustomerRepository _CustomerRepository, ICartRepository _CartRepository)
        {
            this.CustomProductRepository = _CustomProductRepository;
            this.ProductRepository = _ProductRepository;
            this.CustomerRepository = _CustomerRepository;
            this.CartRepository = _CartRepository;
        }

        public class AuthorizeRolesAttribute : AuthorizeAttribute
        {
            public AuthorizeRolesAttribute(params string[] roles)
                : base()
            {
                Roles = string.Join(",", roles);
            }
        }

        /// <summary>
        /// Return Dashboard
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: Choose Template
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult Template()
        {
            List<TemplateViewModel> output = new List<TemplateViewModel>();
            IEnumerable<Template> templates = CustomProductRepository.Templates.Where(x => x.TemplateID != Guid.Empty);
            foreach (Template template in templates)
            {
                TemplateViewModel temp = new TemplateViewModel ();
                temp.TemplateID = template.TemplateID;
                temp.TemplateName = template.TemplateName;
                temp.TemplateSource = template.TemplateSource;
                temp.TemplateDescription = template.TemplateDescription;
                output.Add(temp);
            }
            return View(output);
        }

        /// <summary>
        /// GET: Customize Product
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult Product()
        {
            return View();
        }

        /// <summary>
        /// GET: Customize Product
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ExistingProduct()
        {
            return View();
        }

        /// <summary>
        /// POST : Save Custom Design
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        [HttpPost]
        public JsonResult CreateProduct(ProductInfo input)
        {
            Guid CustomerId = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            Customer customer = CustomerRepository.GetCustomerByID(CustomerId);
            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = input.ProductName,
                ProductCode = CustomProductRepository.CustomProductCodeGenerator,
                ProductCategory = Constant.CUSTOM_PRODUCT_CATEGORY,
                ProductFolderId = Constant.CUSTOM_PRODUCT_FOLDER,
                ProductQuantity = 1,
                TemplateID = input.TemplateID,
                ProductOriginalPrice = input.ProductOriginalPrice,
                ProductRetailPrice = input.ProductRetailPrice,
                TemplateTextureId = input.TextureID
            };

            
            List<Accessory> accessories = new List<Accessory>();
            foreach (var accessory in input.Accessories)
            {
                Accessory temp = new Accessory();
                temp.AccessoriesID = new Guid();
                temp.ProductID = product.ProductId;
                temp.AccessoriesTemplateID = accessory.AccessoriesTemplateID;
                temp.AccessoriesX = accessory.AccessoriesX;
                temp.AccessoriesY = accessory.AccessoriesY;
                accessories.Add(temp);
            }
            product.Accessories = accessories;

            //Add Custom Product Record
            ECWebApp.Domain.CustomProduct customProduct = new ECWebApp.Domain.CustomProduct()
            {
                CustomProductId = Guid.NewGuid(),
                ProductId = product.ProductId,
                CustomerId = customer.CustomerID,
                Specifications = input.Measurements,
                Status = Status.CUSTOM_PRODUCT_ACTIVE,
                CustomProductCreatedOn = DateTime.Now,
                CustomProductCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName
            };

            Image image = new Image()
            {
                ProductImgId = Guid.NewGuid(),
                ProductImageSource = input.ProductImageToByte,
                ProductImageName = product.ProductCode,
                ProductId = product.ProductId,
                CustomProductID = customProduct.CustomProductId,
                IsPrimary = Status.PRODUCT_IMAGE_PRIMARY,
                ProductImageType = input.ProductImageType,
            };
            product.CustomProducts.Add(customProduct);


            CustomProductRepository.AddCustomProduct(product, image, customer, input.Measurements, input.Color);
            ProductInfo output = new ProductInfo()
            {
                ProductID = product.ProductId,
                CustomProductID = customProduct.CustomProductId
            };

            return Json(output, JsonRequestBehavior.AllowGet); 
        }

        public void BuyProduct(ProductInfo input)
        {
            Guid CustomerId = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            Customer customer = CustomerRepository.GetCustomerByID(CustomerId);
            ECWebApp.Domain.CustomProduct product = new ECWebApp.Domain.CustomProduct()
            {
                CustomProductId = Guid.NewGuid(),
                CustomProductCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                CustomProductCreatedOn = DateTime.Now,
                Specifications = input.Measurements,
                CustomerId = CustomerId,
                ProductId = input.ProductID,
                Status = Status.CUSTOM_PRODUCT_ACTIVE,
                
            };
            Image image = new Image()
            {
                ProductImgId = Guid.NewGuid(),
                ProductImageSource = input.ProductImageToByte,
                ProductImageName = input.ProductCode,
                ProductId = input.ProductID,
                CustomProductID = product.CustomProductId,
                IsPrimary = Status.PRODUCT_IMAGE_PRIMARY,
                ProductImageType = input.ProductImageType,
                ProductImageCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                ProductImageCreatedOn = DateTime.Now,
                Status = Status.PRODUCT_IMAGE_ACTIVE
            };
            product.Images.Add(image);


            

            CartItem item = new CartItem()
            {
                ProductId = input.ProductID,
                CartId = CartRepository.GetActiveCart(CustomerId),
                CartListId = Guid.NewGuid(),
                CartListTotalPrice = (decimal)input.ProductRetailPrice,
                CustomProductID = product.CustomProductId,
                CartListTotalQuantity = 1,
                CartListCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                CartListCreatedOn = DateTime.Now,
                CartListUpdatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                CartListUpdatedOn = DateTime.Now
            };
            CustomProductRepository.BuyCustomProduct(product);         
            CartRepository.AddItem(item);

            //return product.ProductId;
        }


        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult SaveDialog(Guid id)
        {
            return View(id);
        }


        [HttpPost]
        public void AddToCart(ProductInfo input)
        {
            Guid CustomerId = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            Customer customer = CustomerRepository.GetCustomerByID(CustomerId);
            CartItem item = new CartItem()
            {
                ProductId = input.ProductID,
                CartId = CartRepository.GetActiveCart(CustomerId),
                CartListId = Guid.NewGuid(),
                CustomProductID = input.CustomProductID,
                CartListTotalQuantity = 1,
                CartListCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                CartListCreatedOn = DateTime.Now,
                CartListUpdatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName,
                CartListUpdatedOn = DateTime.Now
            };

            CartRepository.AddItem(item);
        }
        #region Api Method
        /// <summary>
        /// GET: Custom Product Design
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetCustomProduct(Guid id)
        {
            ProductInfo output = CustomProductRepository.CustomProducts
                .Where(x => x.ProductId.Equals(id))
                .Select(x => new ProductInfo
                {
                    ProductID = x.ProductId,
                    ProductName = x.ProductName,
                    ProductRetailPrice = x.ProductRetailPrice,
                    Color = x.Colors.Where(y => y.ProductId.Equals(id)).Select(y => y.ProductColorValue).FirstOrDefault(),
                    TemplateID = x.TemplateID,
                    TextureID = x.TemplateTextureId,
                    CustomProductID = x.CustomProducts.Where(y => y.ProductId.Equals(id)).Select(y => y.CustomProductId).FirstOrDefault(),
                    ProductCode = x.ProductCode
                })
                .FirstOrDefault();


            return Json(output, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GET: Get Template with TemplateID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTemplate(Guid id)
        {
            TemplateViewModel output = CustomProductRepository.Templates
                .Where(x => x.TemplateID.Equals(id))
                .Select(x => new TemplateViewModel
                {
                    TemplateID = x.TemplateID,
                    TemplateName = x.TemplateName,
                    TemplateSource = x.TemplateSource,
                    TemplatePrice =x.TemplatePrice
                })
                .FirstOrDefault();
            return Json(output, JsonRequestBehavior.AllowGet); 
        }

        /// <summary>
        /// GET: Get lists of textures 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTextures()
        {
            List<TextureInfo> output = CustomProductRepository.Textures
                .Select(x =>  new TextureInfo ()
                {
                    TextureId = x.TextureId,
                    TextureImageByte = x.TextureSource,
                    TextureCode = x.TextureCode,
                    TexturePrice = x.TexturePrice
                })
                .ToList();
           
            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Get lists of textures 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTexture(Guid id)
        {
            TextureInfo output = CustomProductRepository.Textures
                .Where(x => x.TextureId.Equals(id))
                .Select(x => new TextureInfo()
                {
                    TextureId = x.TextureId,
                    TextureImageByte = x.TextureSource,
                    TextureType = x.TextureType,
                    TextureCode = x.TextureCode,
                    TexturePrice = x.TexturePrice
                })
                .FirstOrDefault();
            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Get lists of accessories
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAccessories()
        {
            List<AccessoriesTemplateInfo> output = CustomProductRepository.Accessories
                .Select(x =>
                new AccessoriesTemplateInfo()
                {
                    AccessoriesTemplateId = x.AccssoriesTemplateID,
                    AccessoriesTemplateName = x.AccessoriesTemplateName,
                    AccessoriesTemplateSource = x.AccessoriesTemplateSource,
                    AccessoriesTemplateType = x.AccessoriesTemplateType,
                    AccessoriesTemplatePrice = x.AccessoriesTemplatePrice,
                    AccessoriesTemplateCode = x.AccessoriesTemplateCode
                })
                .ToList();
            
            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: Accessories on existing design 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAccessory(Guid id)
        {
            var output = CustomProductRepository.AccessoryList
                .Where(x => x.ProductID.Equals(id))
                .Select(x =>
                new AccessoriesTemplateInfo()
                {
                    AccessoriesTemplateId = x.AccessoriesTemplateID,
                    AccessoriesID = x.AccessoriesID,
                    AccessoriesTemplateName = x.AccessoriesTemplate.AccessoriesTemplateName,
                    AccessoriesTemplateSource = x.AccessoriesTemplate.AccessoriesTemplateSource,
                    AccessoriesTemplatePrice = x.AccessoriesTemplate.AccessoriesTemplatePrice,
                    AccessoriesTemplateCode = x.AccessoriesTemplate.AccessoriesTemplateCode,
                    AccessoriesX = x.AccessoriesX,
                    AccessoriesY = x.AccessoriesY,
                    AccessoriesTemplateType = x.AccessoriesTemplate.AccessoriesTemplateType
                }).ToList();
           
            return Json(new JavaScriptSerializer().Serialize(output), JsonRequestBehavior.AllowGet);
        }


        
        #endregion
    }
}