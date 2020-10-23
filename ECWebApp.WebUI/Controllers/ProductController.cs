using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using ECWebApp.Domain.Abstract;
using ECWebApp.Domain;
using ECWebApp.WebUI.Models.ViewModel;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.App_Common;
using ECWebApp.Domain.Constant;

namespace ECWebApp.WebUI.Controllers
{
    public class ProductController : Controller
    {

        private ICartRepository CartRepository;
        private IProductRepository ProductRepository;

        public ProductController(ICartRepository _CartRepository, IProductRepository _ProductRepository)
        {
            this.CartRepository = _CartRepository;
            this.ProductRepository = _ProductRepository;
        }

        public class AuthorizeRolesAttribute : AuthorizeAttribute
        {
            public AuthorizeRolesAttribute(params string[] roles) : base()
            {
                Roles = string.Join(",", roles);
            }
        }

        /// <summary>
        /// GET: Product by Category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult List(int category, int page = 1)
        {

            ProductsListViewModel output = new ProductsListViewModel
            {
                Products = ProductRepository.Products
                .Where(x => x.ProductStatus.Equals(Status.PRODUCT_ACTIVE) && x.ProductCategory == category && x.ProductFolderId != Constant.CUSTOM_PRODUCT_FOLDER)
                .Select(x => new ProductInfo()
                {
                    ProductID = x.ProductId,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductRetailPrice = x.ProductRetailPrice,
                    ProductImageByte = x.Images
                                        .Where(y => y.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY)
                                        .Select(y => y.ProductImageSource)
                                        .FirstOrDefault(),
                   ProductImageType = x.Images
                                        .Where(y => y.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY)
                                        .Select(y => y.ProductImageType)
                                        .FirstOrDefault()
                })

            };
            return View(output);
        }

        /// <summary>
        /// Pop up Quick View
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ActionResult PopupProduct(Guid ProductID)
        {
            ProductViewModel output = new ProductViewModel();
            output.ProductColor = ProductRepository.Colors
                            .Where(x => x.ProductId.Equals(ProductID) && x.ProductColorValue != "default");
            Product product = ProductRepository.Products
                            .Where(x => x.ProductId.Equals(ProductID) && x.ProductStatus == Status.PRODUCT_ACTIVE)
                            .FirstOrDefault();
            Image image = ProductRepository.Images
                            .Where(x => x.ProductId.Equals(ProductID) && x.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY && x.Status == Status.PRODUCT_IMAGE_ACTIVE)
                            .FirstOrDefault();
            output.Product = new ProductInfo();
            output.Product.ProductID = product.ProductId;
            output.Product.ProductCode = product.ProductCode;
            output.Product.ProductName = product.ProductName;
            output.Product.ProductDescription = product.ProductDescription;
            output.Product.ProductRetailPrice = product.ProductRetailPrice;
            output.Product.ProductHeight = product.ProductHeight;
            output.Product.ProductLength = product.ProductLength;
            output.Product.ProductWidth = product.ProductWidth;
            output.Product.ProductScale = product.ProductScale;
            output.Product.ProductImageByte = image.ProductImageSource;
            output.Product.ProductImageType = image.ProductImageType;
            return View(output);
        }

        /// <summary>
        /// Full Product Specifications
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public ActionResult ProductDetails(Guid ProductID)
        {
            ProductViewModel output = new ProductViewModel()
            {
                ProductImages = ProductRepository.Images
                .Where(x => x.ProductId.Equals(ProductID) && x.Status == Status.PRODUCT_IMAGE_ACTIVE)
                .Select(x => x.ProductImageSource)

                ,
                ProductColor = ProductRepository.Colors
                .Where(x => x.ProductId.Equals(ProductID))
            };
            Product product = ProductRepository.Products
                           .Where(x => x.ProductId.Equals(ProductID) && x.ProductStatus == Status.PRODUCT_ACTIVE)
                           .FirstOrDefault();
            Image image = ProductRepository.Images
                            .Where(x => x.ProductId.Equals(ProductID) && x.IsPrimary == Status.PRODUCT_IMAGE_PRIMARY && x.Status == Status.PRODUCT_IMAGE_ACTIVE)
                            .FirstOrDefault();
            output.Product = new ProductInfo();
            output.Product.ProductID = product.ProductId;
            output.Product.ProductCode = product.ProductCode;
            output.Product.ProductName = product.ProductName;
            output.Product.ProductDescription = product.ProductDescription;
            output.Product.ProductRetailPrice = product.ProductRetailPrice;
            output.Product.ProductHeight = product.ProductHeight;
            output.Product.ProductLength = product.ProductLength;
            output.Product.ProductWidth = product.ProductWidth;
            output.Product.ProductScale = product.ProductScale;
            output.Product.ProductImageByte = image.ProductImageSource;
            output.Product.ProductImageType = image.ProductImageType;
            return View(output);
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        /// <param name="ProductID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ColorID"></param>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        [HttpPost]
        public void AddToCart(Guid ProductID, Guid CustomerID, string ColorValue)
        {
            CartItem newItem = new CartItem()
            {
                ProductId = ProductID,
                CartId = CartRepository.GetActiveCart(CustomerID),
                CartListId = Guid.NewGuid(),
                CartListTotalQuantity = 1,
                CartListCreatedBy = CustomerID.ToString(),
                CartListCreatedOn = DateTime.Now.ToLocalTime(),
                CartListUpdatedBy = CustomerID.ToString(),
                CartListUpdatedOn = DateTime.Now.ToLocalTime()
            };
            if (ColorValue == null || ColorValue == String.Empty)
            {
                newItem.ProductColorId = ProductRepository.Colors
                    .Where(x => x.ProductId.Equals(ProductID) && x.ProductColorValue.Equals("default"))
                    .Select(x => x.ProductColorId)
                    .FirstOrDefault();
            }
            else
            {
                newItem.ProductColorId = ProductRepository.Colors
                   .Where(x => x.ProductId.Equals(ProductID) && x.ProductColorValue.Equals(ColorValue))
                   .Select(x => x.ProductColorId)
                   .FirstOrDefault();
            }

            CartRepository.AddItem(newItem);

        }

        /// <summary>
        /// Delete Item from Cart
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        [HttpPost]
        public void RemoveFromCart(Guid CartID)
        {
            CartRepository.DeleteItem(CartID);

        }

        /// <summary>
        /// Update Item Quantity in Cart
        /// </summary>
        /// <param name="CartListID"></param>
        /// <param name="ProductQuantity"></param>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        [HttpPost]
        public void UpdateCartList(Guid CartListID, int ProductQuantity)
        {
            CartItem UpdatedCart = new CartItem();
            UpdatedCart.CartListId = CartListID;
            UpdatedCart.CartListTotalQuantity = ProductQuantity;
            CartRepository.UpdateItem(UpdatedCart);
        }
    }
}