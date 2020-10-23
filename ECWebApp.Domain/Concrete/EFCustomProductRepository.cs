using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;


namespace ECWebApp.Domain.Concrete
{
    public class EFCustomProductRepository : ICustomProductRepository
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();

        public string CustomProductCodeGenerator
        {
            get { return "CUS-" + context.Products.Count(x => x.ProductCategory == 1) + 1; }
        }

        public IEnumerable<Product> CustomProducts
        {
            get { return context.Products.Where(x => x.ProductStatus == Constant.Status.CUSTOM_PRODUCT_ACTIVE && x.ProductCategory == Constant.CategoryType.CUSTOM_PRODUCT_CATEGORY); }
        }

        public IEnumerable<vw_PopularAuthor> PopularAuthors
        {
            get { return context.vw_PopularAuthor; }
        }

        public IEnumerable<vw_PopularProduct> PopularProducts
        {
            get { return context.vw_PopularProduct; }
        }

        public IEnumerable<CustomProduct> CustomProductLists
        {
            get { return context.CustomProducts.Where(x => x.Status == Constant.Status.CUSTOM_PRODUCT_ACTIVE); }
        }

        public IEnumerable<Texture> Textures
        {
            get { return context.Textures.Where(x => x.Status == Constant.Status.TEXTURE_ACTIVE && x.TextureId != Guid.Empty); }
        }

        public IEnumerable<AccessoriesTemplate> Accessories
        {
            get { return context.AccessoriesTemplates.Where(x => x.Status == Constant.Status.ACCESSORY_ACTIVE && x.AccssoriesTemplateID != Guid.Empty); }
        }

        public IEnumerable<Accessory> AccessoryList
        {
            get { return context.Accessories; }
        }

        public IEnumerable<AccessoriesTemplateCategory> AccessoriesCategory
        {
            get { return context.AccessoriesTemplateCategories.Where(x => x.Status == Constant.Status.CATEGORY_ACTIVE && x.AccessoriesTemplateCategoryID != Guid.Empty); }
        }

        public IEnumerable<Template> Templates
        {
            get { return context.Templates.Where(x => x.Status == Constant.Status.TEMPLATE_ACTIVE && x.TemplateID != Guid.Empty); }
        }

        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        public void AddCustomProduct(Product product, Image image, Customer customer, string Measurements, string Color)
        {
           
            //Add Product
            product.ProductId = Guid.NewGuid();
            product.ProductStatus = Constant.Status.PRODUCT_ACTIVE;
            product.ProductCreatedOn = DateTime.Now;
            product.ProductCreatedBy = Constant.RoleAssignment.ROLE_ADMIN_NAME;
            foreach (var accessory in product.Accessories)
            {
                accessory.AccessoriesID = Guid.NewGuid();
                accessory.ProductID = product.ProductId;
                accessory.Status = Status.ACCESSORY_ACTIVE;
                accessory.AccessoriesUpdatedOn = DateTime.Now;
                accessory.AccessoriesUpdatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName;
            }
            context.Products.Add(product);

            //Add Default Color
            Color color = new Color()
            {
                ProductId = product.ProductId,
                ProductColorId = Guid.NewGuid(),
                ProductColorValue = Color,
                ProductColorCreatedOn = DateTime.Now,
                ProductColorCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName
            };
            context.Colors.Add(color);



            //Add Primary Image 
            image.ProductImgId = Guid.NewGuid();
            image.ProductId = product.ProductId;
            image.ProductColorId = color.ProductColorId;
            image.Status = Status.PRODUCT_IMAGE_ACTIVE;
            image.IsPrimary = Status.PRODUCT_IMAGE_PRIMARY;
            image.ProductImageCreatedOn = DateTime.Now;
            image.ProductImageCreatedBy = customer.CustomerFirstName + " " + customer.CustomerLastName;
            context.Images.Add(image);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void BuyCustomProduct(CustomProduct product)
        {
            context.CustomProducts.Add(product);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        public void DeleteCustomProduct(Guid ProductID)
        {

            CustomProduct custom = context.CustomProducts.Where(x => x.ProductId.Equals(ProductID) &&
                                                          x.Status == Constant.Status.CUSTOM_PRODUCT_ACTIVE)
                                              .FirstOrDefault();
            custom.Status = Constant.Status.CUSTOM_PRODUCT_INACTIVE;
            custom.CustomProductUpdatedOn = DateTime.Now;
            custom.CustomProductUpdatedBy = custom.CustomProductCreatedBy;


            try
            {
                context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
        }

        public List<Guid> OwnedCustomProducts(Guid CustomerID)
        {
            return context.CustomProducts.Where(x => x.CustomerId==CustomerID).Select(x => x.ProductId).ToList();
        }

        public void CustomProductCreated(Guid CustomProductID)
        {
            CustomProduct customProduct = context.CustomProducts.Where(x => x.CustomProductId.Equals(CustomProductID)).FirstOrDefault();
            customProduct.Status = Status.CUSTOM_PRODUCT_CREATED;
            customProduct.CustomProductCreatedOn = DateTime.Now;
            customProduct.CustomProductCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        public string GetAuthorName(Guid CustomerID)
        {
            var author = context.Customers.Where(x => x.CustomerID.Equals(CustomerID)).FirstOrDefault();
            return author.CustomerFirstName + " " + author.CustomerLastName;
        }

        #region Accessories
        public void AddAccessory(AccessoriesTemplate accessory)
        {
            accessory.AccssoriesTemplateID = Guid.NewGuid();
            accessory.Status = Status.ACCESSORY_ACTIVE;
            accessory.AccessoriesTemplateCreatedOn = DateTime.Now;
            accessory.AccessoriesTemplateCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            context.AccessoriesTemplates.Add(accessory);
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            
        }

        public void UpdateAccessory(AccessoriesTemplate NewAccessory)
        {
            AccessoriesTemplate accessory = context.AccessoriesTemplates
                .Where(x => x.AccssoriesTemplateID.Equals(NewAccessory.AccssoriesTemplateID))
                .FirstOrDefault();
            accessory.AccessoriesTemplateName = NewAccessory.AccessoriesTemplateName;
            accessory.AccessoriesTemplatePrice = NewAccessory.AccessoriesTemplatePrice;
            accessory.AccessoriesTemplateDescription = NewAccessory.AccessoriesTemplateDescription;
            accessory.AccessoriesTemplateCategory = NewAccessory.AccessoriesTemplateCategory;
            if (NewAccessory.AccessoriesTemplateSource != null)
            {
                accessory.AccessoriesTemplateSource = NewAccessory.AccessoriesTemplateSource;
                accessory.AccessoriesTemplateType = NewAccessory.AccessoriesTemplateType;
            }
            accessory.AccessoriesTemplateUpdatedOn = DateTime.Now;
            accessory.AccessoriesTemplateUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void DeleteAccessory(Guid AccessoryID)
        {
            AccessoriesTemplate accessory = context.AccessoriesTemplates
                .Where(x => x.AccssoriesTemplateID.Equals(AccessoryID))
                .FirstOrDefault();
            accessory.Status = Status.ACCESSORY_INACTIVE;
            accessory.AccessoriesTemplateUpdatedOn = DateTime.Now;
            accessory.AccessoriesTemplateUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        #endregion

        #region Accessories Category
        public void AddAccessoryCategory(AccessoriesTemplateCategory category)
        {
            category.AccessoriesTemplateCategoryID = Guid.NewGuid();
            category.Status = Status.ACCESSORY_ACTIVE;
            category.AccessoriesTemplateCategoryCreatedOn = DateTime.Now;
            category.AccessoriesTemplateCategoryCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            context.AccessoriesTemplateCategories.Add(category);
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }
        #endregion

        #region Textures
        public void AddTexture(Texture texture)
        {
            texture.TextureId = Guid.NewGuid();
            texture.Status = Status.TEXTURE_ACTIVE;
            texture.TextureCreatedOn = DateTime.Now;
            texture.TextureCreatedBy = RoleAssignment.ROLE_ADMIN_NAME;
            context.Textures.Add(texture);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void UpdateTexture(Texture NewTexture)
        {
            Texture texture = context.Textures
                .Where(x => x.TextureId.Equals(NewTexture.TextureId))
                .FirstOrDefault();
            texture.TextureCode = NewTexture.TextureCode;
            texture.TextureName = NewTexture.TextureName;
            texture.TextureDescription = NewTexture.TextureDescription;
            if (NewTexture.TextureSource != null)
            {
                texture.TextureSource = NewTexture.TextureSource;
                texture.TextureType = NewTexture.TextureType;
            }
            texture.TextureUpdatedOn = DateTime.Now;
            texture.TextureUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        public void DeleteTexture(Guid TextureID)
        {
            Texture texture = context.Textures
                .Where(x => x.TextureId.Equals(TextureID))
                .FirstOrDefault();
            texture.Status = Status.TEXTURE_INACTIVE;
            texture.TextureUpdatedOn = DateTime.Now;
            texture.TextureUpdatedBy = RoleAssignment.ROLE_ADMIN_NAME;
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
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

        }

        #endregion

    }
}
