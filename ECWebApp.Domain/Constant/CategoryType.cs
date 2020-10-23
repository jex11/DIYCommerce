using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Constant
{
    public class CategoryType
    {
        public static readonly Guid CUSTOMER_CATEGORY = Guid.Parse(ConfigurationManager.AppSettings["CustomerDefaultClass"]);
        public static readonly int CUSTOM_PRODUCT_CATEGORY = Int32.Parse(ConfigurationManager.AppSettings["CustomProductCategory"]);
        public static readonly Guid CUSTOM_PRODUCT_FOLDER = Guid.Parse(ConfigurationManager.AppSettings["CustomProductFolder"]);
    }
}
