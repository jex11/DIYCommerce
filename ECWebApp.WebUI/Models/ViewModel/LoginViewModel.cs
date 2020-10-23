using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "Please fill in your registered email address as username.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}