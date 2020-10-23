using System.Collections.Generic;

using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using System;
using System.Linq;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using System.Diagnostics;


namespace ECWebApp.Domain
{
    public class EFProductRepository : IProductRepository
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();

        public IEnumerable<Product> Products
        {
            get { return context.Products.Where(x => x.ProductStatus == Status.PRODUCT_ACTIVE); }
        }

        public IEnumerable<Color> Colors
        {
            get { return context.Colors; }
        }

        public IEnumerable<Category> Categories
        {
            get { return context.Categories; }
        }

        public IEnumerable<Folder> Folders
        {
            get { return context.Folders; }
        }

        public IEnumerable<Image> Images
        {
            get { return context.Images; }
        }

        public IEnumerable<Promotion> Promotions
        {
            get { return context.Promotions; }
        }

        public IEnumerable<SelectListItem> ScaleList
        {
            get
            {
                List<SelectListItem> Scales = new List<SelectListItem>();
                var data = new[]{
                 new SelectListItem{ Value="1",Text="cm"},
                 new SelectListItem{ Value="2",Text="m"},
                 new SelectListItem{ Value="3",Text="inch"},
                 new SelectListItem{ Value="4",Text="ft"},
             };
                Scales = data.ToList();
                return Scales;
            }
        }

        #region Products
        /// <summary>
        /// Get products according to category and page
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public List<vw_ProductList> GetProducts(Guid CategoryID, int PageIndex, int PageSize, string OrderBy)
        {
            return _GetProducts(CategoryID, PageIndex, PageSize, OrderBy);
        }

        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        private List<vw_ProductList> _GetProducts(Guid CategoryID, int PageIndex, int PageSize, string OrderBy)
        {
            return context.vw_ProductList.Where(x => x.Equals(CategoryID))
                          .Skip(PageIndex * PageSize)
                          .Take(PageSize)
                          .OrderBy(x => x.ProductName)
                          .ToList();
        }

        /// <summary>
        /// Get Product Retail Price 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public decimal? GetProductPrice(Guid ProductID)
        {
            return context.Products.Where(x => x.ProductId.Equals(ProductID)).Select(x => x.ProductRetailPrice).FirstOrDefault();
        }


        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product, Image image)
        {
            //Add Product
            product.ProductId = Guid.NewGuid();
            product.TemplateID = Guid.Empty;
            product.ProductStatus = Constant.Status.PRODUCT_ACTIVE;
            product.ProductCreatedOn = DateTime.Now;
            product.ProductCreatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            context.Products.Add(product);

            //Add Default Color
            Color color = new Color();
            color.ProductId = product.ProductId;
            color.ProductColorId = Guid.NewGuid();
            color.ProductColorValue = "default";
            color.ProductColorCreatedOn = DateTime.Now;
            color.ProductColorCreatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            context.Colors.Add(color);

            //Add Primary Image 
            image.ProductImgId = Guid.NewGuid();
            image.ProductId = product.ProductId;
            image.ProductColorId = color.ProductColorId;
            image.Status = Status.PRODUCT_IMAGE_ACTIVE;
            image.IsPrimary = Status.PRODUCT_IMAGE_PRIMARY;
            image.ProductImageCreatedOn = DateTime.Now;
            image.ProductImageCreatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            context.Images.Add(image);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// Update general details for product
        /// </summary>
        /// <param name="UpdatedProduct"></param>
        public void UpdateProduct(Product UpdatedProduct, Image UpdatedImage)
        {
            
            //Add Product
            Product product = context.Products.Where(x => x.ProductId.Equals(UpdatedProduct.ProductId) &&
                                                          x.ProductStatus == Constant.Status.PRODUCT_ACTIVE)
                                              .FirstOrDefault();
            product.ProductCategory= UpdatedProduct.ProductCategory;
            product.ProductCode = UpdatedProduct.ProductCode;
            product.ProductDescription = UpdatedProduct.ProductDescription;
            product.ProductHeight = UpdatedProduct.ProductHeight;
            product.ProductLength = UpdatedProduct.ProductLength;
            product.ProductWidth = UpdatedProduct.ProductWidth;
            product.ProductWeight = UpdatedProduct.ProductWeight;
            product.ProductScale = UpdatedProduct.ProductScale;
            product.ProductQuantity = UpdatedProduct.ProductQuantity;
            product.ProductName = UpdatedProduct.ProductName;
            product.ProductOriginalPrice = UpdatedProduct.ProductOriginalPrice;
            product.ProductRetailPrice = UpdatedProduct.ProductRetailPrice;
            product.ProductFolderId = UpdatedProduct.ProductFolderId;
            product.ProductUpdatedOn = DateTime.Now;
            product.ProductUpdatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;

            //Add Primary Image 
            
            if (UpdatedImage.ProductImageSource != null)
            {
                Image image = context.Images.Where(x => x.ProductId.Equals(UpdatedProduct.ProductId) &&
                                                          x.Status == Constant.Status.PRODUCT_ACTIVE)
                                              .FirstOrDefault();
                image.ProductImageSource = UpdatedImage.ProductImageSource;
                image.ProductImageType = UpdatedImage.ProductImageType;
                image.ProductImageUpdatedOn = DateTime.Now;
                image.ProductImageUpdatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            }
           

            
            try
            {
                context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

        }


        /// <summary>
        /// Shallow Delete Product
        /// </summary>
        /// <param name="ProductID"></param>
        public void DeleteProduct(Guid ProductID)
        {
            Product product = context.Products.Where(x => x.ProductId.Equals(ProductID) &&
                                                          x.ProductStatus == Constant.Status.PRODUCT_ACTIVE)
                                              .FirstOrDefault();
            product.ProductStatus = Constant.Status.PRODUCT_INACTIVE;
            product.ProductUpdatedOn = DateTime.Now;
            product.ProductUpdatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            try
            {
               context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// Permanently remove products
        /// </summary>
        /// <param name="ProductID"></param>
        public void RemoveProduct(Guid ProductID)
        {
            Product product = context.Products.Where(x => x.ProductId.Equals(ProductID) &&
                                                          x.ProductStatus == Constant.Status.PRODUCT_ACTIVE)
                                              .FirstOrDefault();
            product.ProductUpdatedOn = DateTime.Now;
            product.ProductUpdatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            context.Products.Remove(product);
            try
            {
                context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }
        #endregion

        #region Colors
        //TODO:: Get Colors
        //TODO:: Update Colors Name
        //TODO:: Delete Colors
        //TODO:: Search Colors
        //TODO:: Create Colors
        #endregion

        #region Categories
        //TODO:: Get Categories
        public void AddCategory(Category category)
        {
            category.ProductCategory = context.Categories.Count() + 1;
            category.CategoryStatus = Status.PRODUCT_CATEGORY_ACTIVE;
            category.CategoryCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            category.CategoryCreateOn = DateTime.Now;
            context.Categories.Add(category);
            try
            {
                context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        //TODO:: Update Categories Name
        //TODO:: Delete Categories
        //TODO:: Search Categories
        //TODO:: Create Categories
        #endregion

        #region Folders

        //TODO:: Create Folder
        public void AddFolder(Folder folder)
        {
            folder.ProductFolderId = Guid.NewGuid();
            folder.ProductFolderCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            folder.ProductFolderCreatedOn = DateTime.Now;
            context.Folders.Add(folder);
            try
            {
                context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        //TODO:: Update Folder

        public void UpdateFolder(Folder updatedFolder)
        {
            Folder folder = context.Folders
                .Where(x => x.ProductFolderId.Equals(updatedFolder.ProductFolderId))
                .FirstOrDefault();
            folder.ProductFolderName = updatedFolder.ProductFolderName;
            folder.ProductFolderDescription = updatedFolder.ProductFolderDescription;
            folder.ProductFolderUpdatedOn = DateTime.Now;
            folder.ProductFolderUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            try
            {
                context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        //TODO:: Delete Folder
        public void DeleteFolder(Guid FolderID)
        {
            Folder folder = context.Folders
                .Where(x => x.ProductFolderId.Equals(FolderID))
                .FirstOrDefault();
            folder.Status = Status.FOLDER_INACTIVE;
            folder.ProductFolderUpdatedOn = DateTime.Now;
            folder.ProductFolderUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            try
            {
                context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        //TODO:: Search Folder
        public void SearchFolder(Folder folder)
        {
            //Folder folder = context.Folders
            //    .Where(x => x.ProductFolderId.Equals(updatedFolder.ProductFolderId))
            //    .FirstOrDefault();
            //folder.ProductFolderName = updatedFolder.ProductFolderName;
            //folder.ProductFolderDescription = updatedFolder.ProductFolderDescription;
            //folder.ProductFolderUpdatedOn = DateTime.Now;
            //folder.ProductFolderUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            //try
            //{
            //    context.SaveChangesAsync();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

        }
        #endregion

        #region Images
        //TODO:: Get Images
        public List<Image> GetPrimaryImages(Guid location)
        {
            if (location == null) location = Guid.Empty;
            return context.Images
                        .Where(x => x.IsPrimary == 1 && x.Product.ProductFolderId == location)
                        //.Select(x => x.ProductImageSource  x.ProductImageType })
                        .ToList();
                        
        }

        //TODO:: Update Images Name
        //TODO:: Delete Images
        //TODO:: Search Images
        //TODO:: Create Images
        #endregion

        #region Promotions
        //TODO:: Get Promotions
        //TODO:: Update Promotions Name
        //TODO:: Delete Promotions
        //TODO:: Search Promotions
        //TODO:: Create Promotions
        #endregion
    }
}
