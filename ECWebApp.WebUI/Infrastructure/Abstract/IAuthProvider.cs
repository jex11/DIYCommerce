using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWebApp.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        void SendConfirmationEmail(Customer customer, String subject, String msg);
    }
}