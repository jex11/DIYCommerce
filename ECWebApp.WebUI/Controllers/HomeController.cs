using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using ECWebApp.WebUI.App_Common;
using ECWebApp.WebUI.Infrastructure.Abstract;
using ECWebApp.WebUI.Models.ViewModel;
using Postal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ECWebApp.WebUI.Controllers.COM
{
    public class HomeController : Controller
    {
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;
        private ICartRepository CartRepository;
         private IAuthProvider provider;
         private AuthViewModel customer = new AuthViewModel();

         public HomeController(IProductRepository _ProductRepository, ICustomerRepository _CustomerRepository, ICartRepository _CartRepository, IAuthProvider AuthProvider)
        {
            this.ProductRepository = _ProductRepository;
            this.CustomerRepository = _CustomerRepository;
            this.CartRepository = _CartRepository;
            this.provider = AuthProvider;
        }
       
        /// <summary>
        /// GET: Home Page
        /// </summary>
        /// <returns></returns>        
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// GET: Menu - Category
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCategory()
        {
            ProductCategoryViewModel output = new ProductCategoryViewModel
            {
                //Categories = repository.Categories
                //.Where(x => x.CategoryStatus == "active")
                Categories = ProductRepository.Categories
                .Where(x => x.CategoryStatus.Equals(Status.PRODUCT_CATEGORY_ACTIVE) && x.ProductCategory != Constant.CUSTOM_PRODUCT_CATEGORY)

            };

            return PartialView("Menu", output);
        }

        /// <summary>
        /// GET: Promotion Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Promotion()
        {
            return View();
        }

        /// <summary>
        /// GET: About Us Page
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// GET: Contact Us Page
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUs()
        {
            return View();
        }

        /// <summary>
        /// GET: Widget control for customers
        /// </summary>
        /// <returns></returns>
        public ActionResult Customer()
        {

            if (Session[FormsAuthentication.FormsCookieName] != null && Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName].Value.Equals(Session[FormsAuthentication.FormsCookieName]))
                {
                    Guid UserID = Guid.Parse(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
                    string x = UserID.ToString();
                    customer.Customer = CustomerRepository.GetCustomerByID(UserID);
                    customer.IsLogin = Request.IsAuthenticated;
                    customer.CartCount = CartRepository.CartCount(UserID);
                    Tailor tailor = CustomerRepository.GetTailorByCustomerID(UserID);
                    if (tailor != null)
                    {
                        customer.IsTailor = true;
                        customer.TaskCount = CustomerRepository.TaskCount(tailor.TailorID);
                    }
                    return View(customer);             
                }
            }
            customer.IsLogin = false;
            customer.IsTailor = false;
            return View(customer);
        }

        #region Login Page
        /// <summary>
        /// GET: Login Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string ReturnUrl)
        {
            SigninViewModel output = new SigninViewModel();
            output.ReturnUrl = ReturnUrl;
            return View(output);
        }

        /// <summary>
        /// POST: Login user 
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SigninViewModel Result)
        {
            // TODO:: Detect browser and os
            //var info = HttpContext.Request.UserAgent;   Detect Web Browser and OS

            Result.CustomerCreatedOn = DateTime.Now.ToLocalTime();
            Customer customer = new Customer();
            customer.CustomerEmail = Result.CustomerEmail;
            customer.CustomerPassword = Result.CustomerPassword;
            customer.CustomerCreatedOn = Result.CustomerCreatedOn;

            if (ModelState.IsValid && CustomerRepository.Authenticate(customer, Result.RememberMe))
            {
                String CustomerID = CustomerRepository.GetCustomerID(customer).ToString();
                String[] role = Roles.GetRolesForUser(CustomerID);
                if (role.Contains(RoleAssignment.ROLE_ADMIN_NAME))
                {
                    return RedirectToAction("Index", "Admin_Product", new { Area = "Admin" });
                }
                if (role.Contains(RoleAssignment.ROLE_TAILOR_NAME))
                {
                    return RedirectToLocal(Result.ReturnUrl);
                }
                return RedirectToLocal(Result.ReturnUrl);
            }
            else
            {
                ViewBag.ErrorAuthentication = "Email/Password Invalid. Please try again.";
            }


            return View();
        }

        /// <summary>
        /// GET: Sign out from account
        /// </summary>
        /// <returns></returns>
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            Session[FormsAuthentication.FormsCookieName] = null;
            customer.IsLogin = false;
            customer.Customer = null;

            return RedirectToAction("Home", "Home");
        }
        #endregion

        #region Signup Page
        /// <summary>
        /// GET: Signup Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Signup()
        {
            // Return only page
            return View();
        }

        /// <summary>
        /// POST: Sign Up Form Credentials
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(CustomerViewModel Result)
        {
            if (ModelState.IsValid)
            {
                //post method  .. after u click submit button it willcome here

                Customer customer = new Customer();
                customer.CustomerID = Guid.NewGuid();
                customer.CustomerCategoryID = Constant.CUSTOMER_CATEGORY;
                customer.CustomerFirstName = Result.CustomerFirstName;
                customer.CustomerLastName = Result.CustomerLastName;
                customer.CustomerEmail = Result.CustomerEmail;
                customer.CustomerAddress = Result.CustomerAddress;
                customer.CustomerCity = Result.CustomerCity;
                customer.CustomerContact = Result.CustomerContact;
                customer.CustomerCountry = Result.CustomerCountry;
                customer.CustomerNRIC = Result.CustomerNRIC;
                customer.CustomerPassword = Result.CustomerPassword;
                customer.CustomerPostcode = Result.CustomerPostcode;
                customer.CustomerState = Result.CustomerState;
                //customer.CustomerPoint = 0;
                customer.CustomerStatus = Status.CUSTOMER_PENDING;
                customer.CustomerCreatedOn = DateTime.Now;
                customer.CustomerCreatedBy = customer.CustomerID.ToString();
                
                if (CustomerRepository.SignUp(customer))//this function will send the data to the business layer
                {
                    //TODO:: Return page to sign up success
                    string code = await CustomerRepository.GenerateEmailConfirmationTokenAsync(customer);
                    var callbackUrl = Url.Action("ConfirmEmail", "Customer",
                            new { CustomerID = customer.CustomerID, token = code },
                            Request.Url.Scheme);
                    provider.SendConfirmationEmail(customer, "Confirm your account",
                                           callbackUrl);
                   
                    return RedirectToAction("SignupSuccess", new { id = customer.CustomerID});
                }
                else
                {
                    ViewBag.ErrorMessage = "Sign up process failed. Unknown error detected, please try again few minutes later or contact the administrator if the problem still exists.";
                }

            }
            return RedirectToAction("SignUpPending");
        }

        /// <summary>
        /// GET: Sign up pending page 
        /// </summary>
        /// <returns></returns>
        public ActionResult SignUpPending()
        {
            return View();
        }


        /// <summary>
        /// GET: Sign up success page 
        /// </summary>
        /// <returns></returns>
        public ActionResult SignupSuccess(Guid id)
        {
            CustomerViewModel NewCustomer = new CustomerViewModel();
            NewCustomer.CustomerID = id;
            return View(NewCustomer);
        }
        #endregion

        #region Method
        /// <summary>
        /// Add key to config file dynamically
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddValue(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Minimal);
        }

        /// <summary>
        /// GET: return previous page 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string ReturnUrl)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect("~/" + ReturnUrl);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        /// <summary>
        /// Validate Unique Email
        /// </summary>
        /// <param name="CustomerEmail"></param>
        /// <returns></returns>
        public JsonResult UniqueEmail(string CustomerEmail)
        {
            if (CustomerRepository.ValidateEmail(CustomerEmail))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}