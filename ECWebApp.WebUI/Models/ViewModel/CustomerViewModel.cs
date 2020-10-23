using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class CustomerViewModel
    {
        public Guid CustomerID { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string CustomerPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        public string NewPassword { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Re-enter New Password:")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Password does not match.")]
        public string ConfirmNewPassword { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [System.Web.Mvc.Compare("CustomerPassword", ErrorMessage = "Password does not match.")]
        public string CustomerConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name:")]
        public string CustomerFirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Code:")]
        public int? CustomerCode { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name:")]
        public string CustomerLastName { get; set; }

        [Required(ErrorMessage = "ID No. cannot be empty.")]
        [DataType(DataType.Text)]
        [Display(Name = "IC No. / Passport ID:")]
        public string CustomerNRIC { get; set; }

        [Required(ErrorMessage = "Email Address cannot be empty.")]
        [Remote("UniqueEmail", "Home", ErrorMessage = "This email is already registered.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string CustomerEmail { get; set; }

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

        [Display(Name = "Reward Points:")]
        public int CustomerPoint { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Profile Picture")]
        public string CustomerImgBase64
        {
            get
            {
                if (CustomerImgSource != null)
                {
                    var base64Image = "data:image/" + CustomerImgType + ";base64," + Convert.ToBase64String(CustomerImgSource);
                    return base64Image;
                }

                return null;
            }
        }

        public byte[] CustomerImgSource { get; set; }

        public HttpPostedFileBase CustomerImg { get; set; }

        public string CustomerImgType { get; set; }

        [Display(Name = " Designs.")]
        public int ProductCount { get; set; }

        [Display(Name = " Purchased.")]
        public int PurchasedCount { get; set; }


        [Display(Name = "S/D Ratio")]
        public double SDRatio
        {
            get
            {
                if (PurchasedCount == 0 || ProductCount == 0)
                {
                    return 0;
                }
                return Math.Round((double)PurchasedCount / (double)ProductCount, 2);
            }
        }
    }
}