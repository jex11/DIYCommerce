using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Areas.Admin.Models
{
    public class TailorDetails
    {
        public Guid TailorId { get; set; }

        public string TailorName { get; set; }

        public string TailorEmail { get; set; }

        public string Specialization { get; set; }

        public Nullable<decimal> AvgRating { get; set; }

        public Nullable<int> AvgElapsedTime { get; set; }

        public Nullable<int> OrderInHand { get; set; }

        public Nullable<int> OrderDone { get; set; }

        public Nullable<int> GoodReviews { get; set; }

        public Nullable<int> BadReviews { get; set; }

        public List<OrderAssignment> Likes { get; set; }

        public List<OrderAssignment> Dislikes { get; set; }

        public Nullable<int> Commission { get; set; }
    }
}