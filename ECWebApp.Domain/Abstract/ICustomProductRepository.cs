using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Abstract
{
    public interface ICustomProductRepository
    {
        #region Templates
            IEnumerable<Template> Templates { get; }
        #endregion

        #region Accessories
            IEnumerable<AccessoriesTemplate> Accessories { get; }
            IEnumerable<Accessory> AccessoryList { get; }
            void AddAccessory(AccessoriesTemplate accessory);
            void UpdateAccessory(AccessoriesTemplate accessory);
            void DeleteAccessory(Guid AccessoryID);

        #endregion 
           
        #region Custom Product 
            IEnumerable<vw_PopularAuthor> PopularAuthors { get; }
            IEnumerable<vw_PopularProduct> PopularProducts { get; }
            String CustomProductCodeGenerator { get; }
            IEnumerable<Product> CustomProducts { get; }
            IEnumerable<CustomProduct> CustomProductLists { get; }
            List<Guid> OwnedCustomProducts(Guid CustomerID);
            string GetAuthorName(Guid CustomerID);
            void AddCustomProduct(Product product, Image image, Customer customer, string Measurements, string Color);
            void BuyCustomProduct(CustomProduct product);
            void CustomProductCreated(Guid CustomProductID);
        #endregion

        #region Textures
            IEnumerable<Texture> Textures { get; }
            void AddTexture(Texture texture);
            void UpdateTexture(Texture texture);
            void DeleteTexture(Guid TextureID);
        #endregion

        #region Accessories Category
            IEnumerable<AccessoriesTemplateCategory> AccessoriesCategory { get; }
            void AddAccessoryCategory(AccessoriesTemplateCategory category);
        #endregion

    }
}
