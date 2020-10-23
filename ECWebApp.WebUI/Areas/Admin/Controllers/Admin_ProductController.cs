using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using ECWebApp.WebUI.App_Common;
using ECWebApp.WebUI.Infrastructure.Abstract;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECWebApp.WebUI.Areas.Admin.Controllers
{
    public class Admin_ProductController : Controller
    {
        private IProductRepository ProductRepository;

        public Admin_ProductController(IProductRepository _ProductRepository)
        {

            this.ProductRepository = _ProductRepository;
        }

        /// <summary>
        /// View all products at file directory
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        public ActionResult Index(Guid? id)
        {
            Guid location = new Guid();
            location = Guid.Empty;
            if (id != null)
            {
                location = (Guid)id;
            }

            var productList = ProductRepository.Products.Where(x => x.ProductFolderId == location && x.ProductStatus == Status.PRODUCT_ACTIVE);
            var folderList = ProductRepository.Folders.Where(x => x.ProductFolderFrom == location && x.Status == Status.FOLDER_ACTIVE);
            var imageList = ProductRepository.GetPrimaryImages(location);
            List<ProductInfo> p_list = new List<ProductInfo>();
            List<FolderInfo> f_list = new List<FolderInfo>();

            ProductsListViewModel output = new ProductsListViewModel();
            foreach (var product in productList)
            {
                ProductInfo model = new ProductInfo();
                model.ProductCategory = product.ProductCategory;
                model.ProductCode = product.ProductCode;
                model.ProductImageByte = imageList
                                            .Where(x => x.ProductId.Equals(product.ProductId))
                                            .Select(x => x.ProductImageSource)
                                            .FirstOrDefault();
                model.ProductImageType = imageList
                                            .Where(x => x.ProductId.Equals(product.ProductId))
                                            .Select(x => x.ProductImageType)
                                            .FirstOrDefault();

                model.ProductDescription = product.ProductDescription;
                model.ProductID = product.ProductId;
                model.ProductName = product.ProductName;
                model.ProductRetailPrice = product.ProductRetailPrice;

                p_list.Add(model);
            }
            output.Products = p_list;

            foreach (var folder in folderList)
            {
                FolderInfo model = new FolderInfo();
                model.ProductFolderDescription = folder.ProductFolderDescription;
                model.ProductFolderFrom = folder.ProductFolderFrom;
                model.ProductFolderId = folder.ProductFolderId;
                model.ProductFolderName = folder.ProductFolderName;
                if (model.ProductFolderId == Guid.Empty)
                {
                    
                }
                else
                {
                    f_list.Add(model);
                }


            }
            output.FolderInfo = f_list;
            output.FolderLocation = location;
            output.FolderFrom = ProductRepository.Folders
                .Where(x => x.ProductFolderId == location)
                .Select(x => x.ProductFolderFrom)
                .FirstOrDefault();
            return View(output);
        }


        #region Product Item
        /// <summary>
        /// GET: Add Product page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        public ActionResult AddProduct(Guid id)
        {
            ProductInfo output = new ProductInfo();
            output.FolderID = id;
            output.CategoryList = new SelectList(ProductRepository.Categories.Where(x => x.ProductCategory != Constant.CUSTOM_PRODUCT_CATEGORY), "ProductCategory", "CategoryName");
            output.ScaleList = new SelectList(ProductRepository.ScaleList, "Value", "Text", 1);
            return View(output);
        }

        /// <summary>
        /// POST: create new product in system
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductInfo input)
        {
            Product product = new Product();
            product.ProductCategory = input.ProductCategory;
            product.ProductCode = input.ProductCode;
            product.ProductDescription = input.ProductDescription;
            product.ProductHeight = input.ProductHeight;
            product.ProductLength = input.ProductLength;
            product.ProductWidth = input.ProductWidth;
            product.ProductWeight = input.ProductWeight;
            product.ProductScale = input.ProductScale;
            product.ProductQuantity = input.ProductQuantity;
            product.ProductName = input.ProductName;
            product.ProductOriginalPrice = input.ProductOriginalPrice;
            product.ProductRetailPrice = input.ProductRetailPrice;
            product.ProductFolderId = input.FolderID;
            product.ProductStatus = Status.PRODUCT_ACTIVE;
            input.ProductImageByte = new BinaryReader(input.ProductImage.InputStream).ReadBytes(input.ProductImage.ContentLength);
            Image image = new Image();
            image.ProductImageSource = input.ProductImageResized;
            image.ProductImageType = input.ProductImage.ContentType;
            image.ProductImageName = input.ProductImage.FileName;
            ProductRepository.AddProduct(product, image);
            return RedirectToAction("Index");

        }

        /// <summary>
        /// POST: update product view
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        public ActionResult UpdateProduct(Guid id)
        {
            Product product = ProductRepository.Products
                                .Where(x => x.ProductId.Equals(id))
                                .FirstOrDefault();
            ProductInfo output = new ProductInfo()
            {
                ProductID = product.ProductId,
                ProductCode = product.ProductCode,
                FolderID = (Guid) product.ProductFolderId,
                ProductName = product.ProductName,
                ProductCategory = product.ProductCategory,
                ProductDescription = product.ProductDescription,
                ProductImageByte = product.Images
                                    .Where(x => x.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY)
                                    .Select(x => x.ProductImageSource)
                                    .FirstOrDefault(),
                ProductImageType = product.Images
                                    .Where(x => x.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY)
                                    .Select(x => x.ProductImageType)
                                    .FirstOrDefault(),
                ProductHeight = product.ProductHeight,
                ProductLength = product.ProductLength,
                ProductWidth = product.ProductWidth,
                ProductWeight = product.ProductWeight,
                ProductScale = product.ProductScale,
                ProductOriginalPrice = product.ProductOriginalPrice,
                ProductRetailPrice = product.ProductRetailPrice,
                ProductQuantity = product.ProductQuantity,
                ProductStatus = product.ProductStatus,
                CategoryList = new SelectList(ProductRepository.Categories, "ProductCategory", "CategoryName",product.ProductCategory),
                ScaleList = new SelectList(ProductRepository.ScaleList, "Value", "Text", product.ProductScale)
            };
            return View(output);

        }

        /// <summary>
        /// POST: update product view
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        [HttpPost]
        public ActionResult UpdateProduct(ProductInfo input)
        {
            Product product = new Product();
            product.ProductId = input.ProductID;
            product.ProductCategory = input.ProductCategory;
            product.ProductCode = input.ProductCode;
            product.ProductDescription = input.ProductDescription;
            product.ProductHeight = input.ProductHeight;
            product.ProductLength = input.ProductLength;
            product.ProductWidth = input.ProductWidth;
            product.ProductWeight = input.ProductWeight;
            product.ProductScale = input.ProductScale;
            product.ProductQuantity = input.ProductQuantity;
            product.ProductName = input.ProductName;
            product.ProductOriginalPrice = input.ProductOriginalPrice;
            product.ProductRetailPrice = input.ProductRetailPrice;
            product.ProductFolderId = input.FolderID;

            Image image = new Image();
            if (input.ProductImage != null)
            {
                input.ProductImageByte = new BinaryReader(input.ProductImage.InputStream).ReadBytes(input.ProductImage.ContentLength);
                image.ProductImageSource = input.ProductImageResized;
                image.ProductImageType = input.ProductImage.ContentType;
                image.ProductImageName = input.ProductImage.FileName;
            }
            
            
            ProductRepository.UpdateProduct(product, image);
            return RedirectToAction("Index");

        }

        /// <summary>
        /// POST: update product view
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        [HttpPost]
        public ActionResult DeleteProduct(ProductInfo product)
        {
            ProductRepository.DeleteProduct(product.ProductID);
            return RedirectToAction("Index", new { id = product.FolderLocation});

        }
        #endregion

        #region Folder

        /// <summary>
        /// POST: create new folder in system
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFolder(FolderInfo input)
        {
            Folder folder = new Folder()
            {
                ProductFolderName = input.ProductFolderName,
                ProductFolderFrom = input.ProductFolderFrom,
                ProductFolderDescription = input.ProductFolderDescription,
                Status = Status.FOLDER_ACTIVE
            };
            ProductRepository.AddFolder(folder);
            return RedirectToAction("Index");

        }

        public ActionResult DeleteFolder(Guid id)
        {
            ProductRepository.DeleteFolder(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Category
        /// <summary>
        /// GET: Add category page
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        public PartialViewResult AddCategory(CategoryInfo input)
        {
            return PartialView(input);
        }

        /// <summary>
        /// POST: create new category in system
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_ADMIN_NAME)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CategoryInfo input)
        {
            Category category = new Category()
            {
                CategoryName = input.CategoryName,
                CategoryDescription = input.CategoryDescription
            };
            ProductRepository.AddCategory(category);
            return RedirectToAction("AddProduct", new { id = input.FolderID});

        }
        #endregion

    }
}