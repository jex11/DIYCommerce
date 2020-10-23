using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Constant
{
    public class Status
    {
        #region Order Status
        public const int ORDER_INACTIVE = 0;
        public const int ORDER_PENDING = 1;
        public const int ORDER_IN_PROGRESS = 2;
        public const int ORDER_DELIVERED = 3;
        public const int ORDER_DONE = 4;
        public const int ORDER_FAILED = 5;

        public static string OrderStatusConverter(int x)
        {
            switch (x)
            {
                case 0: return "Inactive";
                case 1: return "Pending";
                case 2: return "In Progress";
                case 3: return "Delivered";
                case 4: return "Done";
                default: return "Failed";
            }
        }
        #endregion

        #region Cart Status
        public const int CART_INACTIVE = 0;
        public const int CART_PENDING = 1;
        public const int CART_SUCCESS = 2;
        public const int CART_FAILED = 3;
        #endregion

        #region Payment Status
        public const int PAYMENT_INACTIVE = 0;
        public const int PAYMENT_PENDING = 1;
        public const int PAYMENT_SUCCESS = 2;
        public const int PAYMENT_FAILED = 3;
        #endregion

        #region Category Status
        public const int CATEGORY_INACTIVE = 0;
        public const int CATEGORY_ACTIVE = 1;
        #endregion

        #region Customer Status
        public const int CUSTOMER_INACTIVE = 0;
        public const int CUSTOMER_ACTIVE = 1;
        public const int CUSTOMER_PENDING = 2;
        #endregion

        #region Customer Confirmation Token Status
        public const int CUSTOMER_CONFIRMATION_TOKEN_USED = 0;
        public const int CUSTOMER_CONFIRMATION_TOKEN_ACTIVE = 1;
        public const int CUSTOMER_CONFIRMATION_TOKEN_EXPIRED = 2;
        #endregion

        #region Customer Class Status
        public const int CUSTOMER_CLASS_INACTIVE = 0;
        public const int CUSTOMER_CLASS_ACTIVE = 1;
        #endregion

        #region  Customer Role Status
        public const int CUSTOMER_ROLE_INACTIVE = 0;
        public const int CUSTOMER_ROLE_ACTIVE = 1;
        #endregion

        #region Customer Adress Status
        public const int CUSTOMER_ADDRESS_ACTIVE = 1;
        public const int CUSTOMER_ADDRESS_INACTIVE = 2;
        #endregion

        #region Product Category Status
        public const int PRODUCT_CATEGORY_INACTIVE = 0;
        public const int PRODUCT_CATEGORY_ACTIVE = 1;
        #endregion

        #region Product Color OutOfStock Status
        public const int PRODUCT_COLOR_STOCK_UNAVAILABLE = 0;
        public const int PRODUCT_COLOR_STOCK_AVAILABLE = 1;
        public const int PRODUCT_COLOR_STOCK_PREORDER = 2;
        #endregion

        #region Product Status
        public const int PRODUCT_INACTIVE = 0;
        public const int PRODUCT_ACTIVE = 1;
        #endregion

        #region Folder Status
        public const int FOLDER_INACTIVE = 0;
        public const int FOLDER_ACTIVE = 1;
        #endregion

        #region Product Image Status / IsPrimary Status
        public const int PRODUCT_IMAGE_INACTIVE = 0;
        public const int PRODUCT_IMAGE_ACTIVE = 1;
        public const int PRODUCT_IMAGE_NORMAL = 0;
        public const int PRODUCT_IMAGE_PRIMARY = 1;
        #endregion

        #region Product Promotion Status
        public const int PRODUCT_PROMOTION_INACTIVE = 0;
        public const int PRODUCT_PROMOTION_ACTIVE = 1;
        #endregion

        #region Custom Template Status
        public const int TEMPLATE_INACTIVE = 0;
        public const int TEMPLATE_ACTIVE = 1;
        #endregion

        #region Custom Texture Status
        public const int TEXTURE_INACTIVE = 0;
        public const int TEXTURE_ACTIVE = 1;
        #endregion

        #region Custom Accessory Status
        public const int ACCESSORY_INACTIVE = 0;
        public const int ACCESSORY_ACTIVE = 1;
        #endregion

        #region Custom Product Status
        public const int CUSTOM_PRODUCT_INACTIVE = 0;
        public const int CUSTOM_PRODUCT_ACTIVE = 1;
        public const int CUSTOM_PRODUCT_CREATED = 2;
        public const int CUSTOM_PRODUCT_PURCHASE_ACTIVE = 2;
        #endregion

    }
}
