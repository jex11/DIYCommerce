using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.Admin.Models
{
    public class CustomerDetails
    {
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string ICNo { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; }

        public Nullable<int> CustomerPoint { get; set; }
    }
}