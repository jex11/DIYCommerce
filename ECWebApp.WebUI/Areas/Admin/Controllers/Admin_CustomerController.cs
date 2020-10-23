using ECWebApp.Domain.Abstract;
using ECWebApp.WebUI.Areas.Admin.Models;
using ECWebApp.WebUI.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWebApp.WebUI.Areas.Admin.Controllers
{
    public class Admin_CustomerController : Controller
    {
        private ICustomerRepository CustomerRepository;

        public Admin_CustomerController(ICustomerRepository _CustomerRepository)
        {
            this.CustomerRepository = _CustomerRepository;
        }

        // GET: Admin/Admin_Customer
        public ActionResult Index()
        {
            CustomerListViewModel output = new CustomerListViewModel();
            output.CustomerInfo = new List<CustomerDetails>();
            List<CustomerDetails> customerDetailList = new List<CustomerDetails>();
            var lstCustomer = CustomerRepository.DesignCustomers;
            foreach (var customer in lstCustomer)
            {
                CustomerDetails customerdetail = new CustomerDetails();
                customerdetail.CustomerId = customer.CustomerID;
                customerdetail.CustomerName = customer.CustomerFirstName + ' ' + customer.CustomerLastName;
                customerdetail.ICNo = customer.CustomerNRIC;
                customerdetail.Email = customer.CustomerEmail;
                customerdetail.Contact = customer.CustomerContact;
                customerdetail.CustomerPoint = customer.CustomerPoint;
                customerDetailList.Add(customerdetail);
            }
            output.CustomerInfo = customerDetailList;
            return View(output);
        }
    }
}