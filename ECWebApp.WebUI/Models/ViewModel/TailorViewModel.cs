using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class TailorViewModel
    {
        public Guid TailorID { get; set; }
        public Guid CustomerID { get; set; }

        [Required(ErrorMessage = "Specialization cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Specialization:")]
        public string Specialization { get; set; }

        public decimal? AverageRating { get; set; }

        public int? AverageElapsedDay { get; set; }

        public int? OrderDone { get; set; }

        [Required(ErrorMessage = "Order In Hand cannot be empty.")]
        [Display(Name = "Order in Hand:")]
        public int? OrderInHand { get; set; }
    }
}