using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class AddressViewModel
    {
        public Guid AddressId { get; set; }
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Address Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Address Name:")]
        public string CustomerAddressName { get; set; }

        [Required(ErrorMessage = "Address cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Address:")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Post Code cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Postcode:")]
        public string CustomerPostcode { get; set; }

        [Required(ErrorMessage = "City cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "City:")]
        public string CustomerCity { get; set; }

        [Required(ErrorMessage = "State cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "State:")]
        public string CustomerState { get; set; }

        [Required(ErrorMessage = "Country cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Country:")]
        public string CustomerCountry { get; set; }

        [Display(Name = "Contact No.:")]
        public string CustomerContact { get; set; }
        
    }
}