using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Infrastructure.Abstract;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using ECWebApp.Domain;
using Postal;

namespace ECWebApp.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {

        public void SendConfirmationEmail(Customer customer, String subject, String link)
        {
            dynamic email = new Email("SignUpConfirmationEmail");
            email.To = customer.CustomerEmail;
            email.Subject = subject;
            email.RegisterLink = link;
            email.Send();
        }
    }
}