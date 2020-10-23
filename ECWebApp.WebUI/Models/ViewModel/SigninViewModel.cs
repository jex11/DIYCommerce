using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class SigninViewModel
    {
        [Required(ErrorMessage = "You forgot to fill in your username. Use e-mail as your username for login.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                           ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Username:")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "You forgot to fill in your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string CustomerPassword { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public DateTime CustomerCreatedOn { get; set; }

        public string ConfirmEmailToken { get; set; }

        public string ReturnUrl { get; set; }
    }
}