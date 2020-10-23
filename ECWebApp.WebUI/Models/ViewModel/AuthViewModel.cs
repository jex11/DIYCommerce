using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Models.ViewModel
{
    public class AuthViewModel
    {
        public Customer Customer { get; set; }
        public bool? IsLogin { get; set; }
        public bool? IsTailor { get; set; }
        public int CartCount { get; set; }
        public int TaskCount { get; set; }
    }
}