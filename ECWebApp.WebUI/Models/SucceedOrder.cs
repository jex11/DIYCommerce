using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models
{
    public class SucceedOrder
    {
        public Guid orderID;
        public Guid succeedcartID;
        public Guid cartItemID;
        public Guid productID;
        public Guid templateID;
        public int orderStatus;
        public DateTime orderDeadline;
        public int cartStatus;
        public int totalQuantity;
        public DateTime orderBeginTime;
        public DateTime orderFinishedTime;
         
    }
}