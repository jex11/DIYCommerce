using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using ECWebApp.WebUI.Areas.Admin.Models;
using ECWebApp.WebUI.Areas.Admin.Models.ViewModels;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.Admin.Controllers
{
    public class Admin_CustomProductController : Controller
    {
        private ICustomProductRepository CustomProductRepository;
        private ICartRepository CartRepository;
        private ICustomerRepository CustomerRepository;

        public Admin_CustomProductController(ICustomProductRepository _CustomProductRepository, ICartRepository _CartRepository, ICustomerRepository _CustomerRepository)
        {

            this.CustomProductRepository = _CustomProductRepository;
            this.CartRepository = _CartRepository;
            this.CustomerRepository = _CustomerRepository;
        }

        /// <summary>
        /// GET: Main page for custom product management
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region Custom Products
        public ActionResult CustomProducts()
        {
            List<ProductInfo> output = CustomProductRepository.CustomProductLists
                .Join(CartRepository.PendingCartItems.Where(x => x.Cart.PaymentStatus.Equals(Status.PAYMENT_PENDING)),
                x => x.CustomProductId,
                y => y.CustomProductID,
                (x, y) =>
                new ProductInfo()
                {
                    ProductID = x.ProductId,
                    CustomProductID = x.CustomProductId,
                    ProductCode = x.Product.ProductCode,
                    Measurements = x.Specifications,
                    ProductImageByte = x.Images.Select(z => z.ProductImageSource).FirstOrDefault(),
                    ProductImageType = x.Images.Select(z => z.ProductImageType).FirstOrDefault(),
                    ProductName = x.Product.ProductName,
                    ProductRetailPrice = x.Product.ProductRetailPrice,
                    ProductCategory = x.Product.ProductCategory,
                    ProductQuantity = y.CartListTotalQuantity,
                    CustomProductSoldOn = y.CartListCreatedOn.ToString("MM/dd/yyyy")
                })
                .OrderBy(x => x.CustomProductCreatedOn)
                .ToList();


            return View(output);
        }

        public ActionResult DetailCustomProducts(Guid id)
        {
            ProductInfo output = CustomProductRepository.CustomProductLists
                                                .Where(x => x.CustomProductId.Equals(id))
                                                .Select(x => new ProductInfo(){
                                                    CustomProductID = x.CustomProductId,
                                                    ProductCode = x.Product.ProductCode,
                                                    ProductName = x.Product.ProductName,
                                                    Measurements = x.Specifications,
                                                    ProductRetailPrice = x.Product.ProductRetailPrice,
                                                    ProductImageByte = x.Images.Where(y => y.CustomProductID.Equals(x.CustomProductId)).Select(y => y.ProductImageSource).FirstOrDefault(),
                                                    ProductImageType = x.Images.Where(y => y.CustomProductID.Equals(x.CustomProductId)).Select(y => y.ProductImageType).FirstOrDefault(),
                                                    ProductDescription = x.Product.ProductDescription
                                                })
                                                .FirstOrDefault();


            return View(output);
        }

        public JsonResult CustomProductBuyer(Guid id)
        {
            CustomerViewModel output = CustomerRepository.Customers
                                                .Where(x => x.CustomProducts.Select(y => y.CustomProductId).Contains(id))
                                                .Select(x => new CustomerViewModel()
                                                {
                                                   CustomerID = x.CustomerID,
                                                   CustomerFirstName = x.CustomerFirstName,
                                                   CustomerLastName = x.CustomerLastName,
                                                   CustomerAddress = x.CustomerAddress,
                                                   CustomerCity = x.CustomerCity,
                                                   CustomerPostcode = x.CustomerPostcode,
                                                   CustomerState = x.CustomerState,
                                                   CustomerCountry = x.CustomerCountry,
                                                   CustomerContact = x.CustomerContact,
                                                   CustomerEmail = x.CustomerEmail
                                                })
                                                .FirstOrDefault();


            return Json(output, JsonRequestBehavior.AllowGet); 
        }

        public void CustomProductDone(Guid id)
        {
            CustomProductRepository.CustomProductCreated(id);
        }


        #endregion


        #region Textures
        /// <summary>
        /// GET: Table for textures
        /// </summary>
        /// <returns></returns>
        public ActionResult Textures()
        {
            TextureViewModel output = new TextureViewModel();
            output.Textures = new List<TextureInfo>();
            foreach(var texture in CustomProductRepository.Textures){
                TextureInfo temp = new TextureInfo()
                {
                    TextureId = texture.TextureId,
                    TextureCode = texture.TextureCode,
                    TextureName = texture.TextureName,
                    TextureDescription = texture.TextureDescription,
                    TexturePrice = texture.TexturePrice,
                    TextureImageByte = texture.TextureSource,
                    TextureType = texture.TextureType
                };
                output.Textures.Add(temp);
            }

            return View(output);
        }

        /// <summary>
        /// GET: Add textures dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTexture(){
            return View();
        }

        /// <summary>
        /// POST: Add texture
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddTexture(TextureInfo texture)
        {
            Texture input = new Texture()
            {
                TextureCode = texture.TextureCode,
                TextureName = texture.TextureName,
                TextureDescription = texture.TextureDescription,
                TexturePrice = texture.TexturePrice,
                TextureSource = new BinaryReader(texture.TextureImage.InputStream).ReadBytes(texture.TextureImage.ContentLength),
                TextureType = texture.TextureImage.ContentType.ToString(),
            };

            CustomProductRepository.AddTexture(input);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Edit Textures dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTexture(Guid id)
        {
            Texture texture = CustomProductRepository.Textures
                                                .Where(x => x.TextureId.Equals(id))
                                                .FirstOrDefault();
            TextureInfo output = new TextureInfo()
            {
                TextureCode = texture.TextureCode,
                TextureName = texture.TextureName,
                TextureDescription = texture.TextureDescription,
                TexturePrice = texture.TexturePrice,
                TextureImageByte = texture.TextureSource,
                TextureId = texture.TextureId,
                TextureType = texture.TextureType
               
            };
            return View(output);
        }

        /// <summary>
        /// POST: Edit Textures
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTexture(TextureInfo texture)
        {
            Texture input = new Texture()
            {

                TextureCode = texture.TextureCode,
                TextureName = texture.TextureName,
                TextureDescription = texture.TextureDescription,
                TexturePrice = texture.TexturePrice,
                TextureId = texture.TextureId,
                TextureType = texture.TextureImage.ContentType.ToString(),
            };

            if (texture.TextureImage != null)
            {
                input.TextureSource = new BinaryReader(texture.TextureImage.InputStream).ReadBytes(texture.TextureImage.ContentLength);
            }

            CustomProductRepository.UpdateTexture(input);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Details textures dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailTexture(Guid id){

            var texture = CustomProductRepository.Textures.Where(x => x.TextureId.Equals(id)).FirstOrDefault();
            TextureInfo output = new TextureInfo()
            {
                TextureCode = texture.TextureCode,
                TextureName = texture.TextureName,
                TextureDescription = texture.TextureDescription,
                TextureImageByte = texture.TextureSource,
                TexturePrice = texture.TexturePrice,
                TextureId = texture.TextureId,
                TextureType = texture.TextureType
            };
            return View(output);
        }


        public ActionResult ConfirmationDeleteTexture(Guid id)
        {
            var texture = CustomProductRepository.Textures.Where(x => x.TextureId.Equals(id)).FirstOrDefault();
            TextureInfo output = new TextureInfo()
            {
                TextureId = texture.TextureId,
                TextureName = texture.TextureName
            };
            return View(output);
        }

        /// <summary>
        /// GET: delete textures dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteTexture(Guid id){
            CustomProductRepository.DeleteTexture(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Accessories
        /// <summary>
        /// GET: Accessories Table
        /// </summary>
        /// <returns></returns>
        public ActionResult Accessories()
        {
            AccessoriesViewModel output = new AccessoriesViewModel();
            output.Accessories = new List<AccessoriesTemplateInfo>();
            foreach (var accessory in CustomProductRepository.Accessories)
            {
                AccessoriesTemplateInfo temp = new AccessoriesTemplateInfo()
                {
                    AccessoriesTemplateId = accessory.AccssoriesTemplateID,
                    AccessoriesTemplateName = accessory.AccessoriesTemplateName,
                    AccessoriesTemplateCode = accessory.AccessoriesTemplateCode,
                    AccessoriesTemplatePrice = accessory.AccessoriesTemplatePrice,
                    AccessoriesTemplateDescription = accessory.AccessoriesTemplateDescription,
                    AccessoriesTemplateCategoryName = accessory.AccessoriesTemplateCategory1.AccessoriesTemplateCategoryName,
                    AccessoriesTemplateSource = accessory.AccessoriesTemplateSource,  /* Small Image*/
                    AccessoriesTemplateType = accessory.AccessoriesTemplateType
                };
                output.Accessories.Add(temp);
            }

            return View(output);
        }

        /// <summary>
        /// GET: Add Accessories dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AddAccessory()
        { 
            AccessoriesTemplateInfo output = new AccessoriesTemplateInfo()
            {
                CategoryList = new SelectList(CustomProductRepository.AccessoriesCategory, "AccessoriesTemplateCategoryID", "AccessoriesTemplateCategoryName")
            };

            return View(output);
        }

        /// <summary>
        /// POST: Add Accessories
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAccessory(AccessoriesTemplateInfo accessory)
        {
           

            AccessoriesTemplate input = new AccessoriesTemplate()
            {
                AccessoriesTemplateName = accessory.AccessoriesTemplateName,
                AccessoriesTemplateSource = new BinaryReader(accessory.AccessoriesTemplateImage.InputStream).ReadBytes(accessory.AccessoriesTemplateImage.ContentLength),
                AccessoriesTemplateType = accessory.AccessoriesTemplateImage.ContentType.ToString(),
                AccessoriesTemplateCode = accessory.AccessoriesTemplateCode,
                AccessoriesTemplatePrice = accessory.AccessoriesTemplatePrice,
                AccessoriesTemplateCategory = accessory.AccessoriesTemplateCategory,
                AccessoriesTemplateDescription = accessory.AccessoriesTemplateDescription
            };

            CustomProductRepository.AddAccessory(input);
            return RedirectToAction("Index");
        }


        /// <summary>
        /// GET: Edit Accessories dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditAccessory(Guid id)
        {
            AccessoriesTemplate accessory = CustomProductRepository.Accessories
                                                .Where(x => x.AccssoriesTemplateID.Equals(id))
                                                .FirstOrDefault();
            AccessoriesTemplateInfo output = new AccessoriesTemplateInfo()
            {
                AccessoriesTemplateName = accessory.AccessoriesTemplateName,
                AccessoriesTemplateDescription = accessory.AccessoriesTemplateDescription,
                AccessoriesTemplateSource = accessory.AccessoriesTemplateSource,
                AccessoriesTemplateCode = accessory.AccessoriesTemplateCode,
                AccessoriesTemplatePrice = accessory.AccessoriesTemplatePrice,
                AccessoriesTemplateId = accessory.AccssoriesTemplateID,
                AccessoriesTemplateType = accessory.AccessoriesTemplateType,
                AccessoriesTemplateCategory = accessory.AccessoriesTemplateCategory,
                CategoryList = new SelectList(CustomProductRepository.AccessoriesCategory, "AccessoriesTemplateCategoryID", "AccessoriesTemplateCategoryName")
            };
            return View(output);
        }

        /// <summary>
        /// POST: Edit Accessories
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAccessory(AccessoriesTemplateInfo accessory)
        {
            AccessoriesTemplate input = new AccessoriesTemplate()
            {
                AccessoriesTemplateName = accessory.AccessoriesTemplateName,
                AccssoriesTemplateID = accessory.AccessoriesTemplateId,
                AccessoriesTemplateCode = accessory.AccessoriesTemplateCode,
                AccessoriesTemplatePrice = accessory.AccessoriesTemplatePrice,
                AccessoriesTemplateCategory = accessory.AccessoriesTemplateCategory,
                AccessoriesTemplateDescription = accessory.AccessoriesTemplateDescription
            };

            if (accessory.AccessoriesTemplateImage != null)
            {
                input.AccessoriesTemplateSource = new BinaryReader(accessory.AccessoriesTemplateImage.InputStream).ReadBytes(accessory.AccessoriesTemplateImage.ContentLength);
                input.AccessoriesTemplateType = accessory.AccessoriesTemplateImage.ContentType.ToString();
            }

            CustomProductRepository.UpdateAccessory(input);
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Accessories Details dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailAccessory(Guid id)
        {
            var accessory = CustomProductRepository.Accessories.Where(x => x.AccssoriesTemplateID.Equals(id)).FirstOrDefault();
            AccessoriesTemplateInfo output = new AccessoriesTemplateInfo()
            {
                AccessoriesTemplateName = accessory.AccessoriesTemplateName,
                AccessoriesTemplateDescription = accessory.AccessoriesTemplateDescription,
                AccessoriesTemplateSource = accessory.AccessoriesTemplateSource,
                AccessoriesTemplateCode = accessory.AccessoriesTemplateCode,
                AccessoriesTemplatePrice = accessory.AccessoriesTemplatePrice,
                AccessoriesTemplateId = accessory.AccssoriesTemplateID,
                AccessoriesTemplateCategoryName = accessory.AccessoriesTemplateCategory1.AccessoriesTemplateCategoryName,
                AccessoriesTemplateType = accessory.AccessoriesTemplateType 
            };
            return View(output);
        }

        /// <summary>
        /// GET: Delete Accessories dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmationDeleteAccessory(Guid id)
        {
            var accessory = CustomProductRepository.Accessories.Where(x => x.AccssoriesTemplateID.Equals(id)).FirstOrDefault();
            AccessoriesTemplateInfo output = new AccessoriesTemplateInfo()
            {
                AccessoriesTemplateId = accessory.AccssoriesTemplateID,
                AccessoriesTemplateName = accessory.AccessoriesTemplateName
            };
            return View(output);
        }

        /// <summary>
        /// GET: Delete Accessories
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAccessory(Guid id)
        {
            CustomProductRepository.DeleteAccessory(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Accessories Category
        public ActionResult AddAccessoryCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccessoryCategory(AccessoryCategoryInfo category)
        {
            AccessoriesTemplateCategory input = new AccessoriesTemplateCategory()
            {
                AccessoriesTemplateCategoryName = category.AccessoriesTemplateCategoryName,
                AccessoriesTemplateCategoryDescription = category.AccessoriesTemplateCategoryDescription
            };

            CustomProductRepository.AddAccessoryCategory(input);
            return RedirectToAction("AddAccessory");
        }
        #endregion
        
    }
    
}