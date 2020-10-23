using ECWebApp.Domain;
using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using ECWebApp.WebUI.Infrastructure.Abstract;
using ECWebApp.WebUI.Models;
using ECWebApp.WebUI.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECWebApp.WebUI.Controllers.COM
{
    public class CustomerController : Controller
    {
        private ICartRepository CartRepository;
        private IProductRepository ProductRepository;
        private ICustomerRepository CustomerRepository;
        private IAuthProvider provider;
        private IOrderRepository OrderRepository;
        public htAfinnList affinlist = new htAfinnList();

        public CustomerController(ICartRepository _CartRepository, IProductRepository _ProductRepository, ICustomerRepository _CustomerRepository, IAuthProvider AuthProvider, IOrderRepository _OrderRepository)
        {
            this.CartRepository = _CartRepository;
            this.ProductRepository = _ProductRepository;
            this.CustomerRepository = _CustomerRepository;
            this.provider = AuthProvider;
            this.OrderRepository = _OrderRepository;
        }

        public class AuthorizeRolesAttribute : AuthorizeAttribute
        {
            public AuthorizeRolesAttribute(params string[] roles) : base()
            {
                Roles = string.Join(",", roles);
            }
        }

        #region Customer Page
        /// <summary>
        /// GET: Customer Page Layout
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult CustomerPage()
        {
            return View();
        }

        #region Account Info
        /// <summary>
        /// GET: Account Info tab
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult AccountInfo(Guid CustomerID)
        {
            var Customer = CustomerRepository.GetCustomerByID(CustomerID);
            CustomerViewModel output = new CustomerViewModel();
            output.CustomerID = CustomerID;
            output.CustomerFirstName = Customer.CustomerFirstName;
            output.CustomerImgSource = Customer.CustomerImg;
            output.CustomerLastName = Customer.CustomerLastName;
            output.CustomerEmail = Customer.CustomerEmail;
            output.CustomerContact = Customer.CustomerContact;
            output.CustomerPassword = Customer.CustomerPassword;
            output.CustomerPoint = (int)Customer.CustomerPoint;
            return View(output);
        }

        /// <summary>
        /// GET: Change Username tab page
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangeUserName(Guid CustomerID)
        {
            return View();
        }

        /// <summary>
        /// POST: Change Username
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangeUserName(Guid CustomerID, CustomerViewModel input)
        {
            Customer UpdatedCustomer = new Customer();
            UpdatedCustomer.CustomerID = CustomerID;
            UpdatedCustomer.CustomerFirstName = input.CustomerFirstName;
            UpdatedCustomer.CustomerLastName = input.CustomerLastName;
            CustomerRepository.UpdateCustomerUsername(UpdatedCustomer);

            return RedirectToAction("CustomerPage");
        }

        /// <summary>
        /// GET: ChangeEmail Tab page
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangeEmail(Guid CustomerID)
        {
            return View();
        }

        /// <summary>
        /// POST: Change Email
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangeEmail(Guid CustomerID, CustomerViewModel input)
        {
            Customer UpdatedCustomer = new Customer();
            UpdatedCustomer.CustomerID = CustomerID;
            UpdatedCustomer.CustomerEmail = input.CustomerEmail;
            CustomerRepository.UpdateCustomerEmail(UpdatedCustomer);

            return RedirectToAction("CustomerPage");
        }

        /// <summary>
        /// GET: ChangePassword Tab page
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangePassword(Guid CustomerID)
        {
            return View();
        }

        /// <summary>
        /// POST: Change Password
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangePassword(Guid CustomerID, CustomerViewModel input)
        {
            Customer UpdatedCustomer = new Customer();
            UpdatedCustomer.CustomerID = CustomerID;
            UpdatedCustomer.CustomerPassword = input.CustomerPassword;
            CustomerRepository.UpdateCustomerPassword(UpdatedCustomer);

            return RedirectToAction("CustomerPage");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult ChangeProfileImage(CustomerViewModel input)
        {
            Customer customer = new Customer()
            {
                CustomerID = input.CustomerID,
                CustomerImg = new BinaryReader(input.CustomerImg.InputStream).ReadBytes(input.CustomerImg.ContentLength),
                CustomerImgType = input.CustomerImg.ContentType
            };
            CustomerRepository.UpdateCustomerProfileImage(customer);
            return RedirectToAction("Customer");
        }

        #endregion

        #region Address Book
        /// <summary>
        /// GET: Address Book tab
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult AddressBook(Guid CustomerID)
        {
            var addresses = CustomerRepository.GetAddresses(CustomerID);
            Address address1 = new Address();

            List<AddressViewModel> output = new List<AddressViewModel>();
            foreach (var address in addresses)
            {
                AddressViewModel model = new AddressViewModel();
                model.AddressId = address.AddressId;
                model.CustomerAddress = address.Address1;
                model.CustomerCity = address.City;
                model.CustomerContact = address.Contact;
                model.CustomerCountry = address.Country;
                model.CustomerId = CustomerID;
                model.CustomerAddressName = address.Name;
                model.CustomerPostcode = address.Postcode;
                model.CustomerState = address.State;

                output.Add(model);
            }
            return View(output);
        }

        /// <summary>
        /// GET: Address Detail in Dialog
        /// </summary>
        /// <param name="AddressID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult UpdateAddressBook(Guid AddressID)
        {
            Address Address = CustomerRepository.GetAddress(AddressID);

            AddressViewModel output = new AddressViewModel();

            output.CustomerId = Address.CustomerId;
            output.AddressId = AddressID;
            output.CustomerAddress = Address.Address1;
            output.CustomerCity = Address.City;
            output.CustomerContact = Address.Contact;
            output.CustomerCountry = Address.Country;
            output.CustomerPostcode = Address.Postcode;
            output.CustomerState = Address.State;
            return View(output);

        }

        /// <summary>
        /// POST: Update Address in Dialog
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="input"></param>
        /// <param name="AddressID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult UpdateAddressBook(Guid AddressID, AddressViewModel input)
        {
            Address UpdatedAddress = new Address();
            UpdatedAddress.CustomerId = input.CustomerId;
            UpdatedAddress.AddressId = AddressID;
            UpdatedAddress.Name = input.CustomerAddressName;
            UpdatedAddress.Address1 = input.CustomerAddress;
            UpdatedAddress.State = input.CustomerState;
            UpdatedAddress.City = input.CustomerCity;
            UpdatedAddress.Postcode = input.CustomerPostcode;
            UpdatedAddress.Country = input.CustomerCountry;
            UpdatedAddress.Contact = input.CustomerCountry;

            CustomerRepository.UpdateAddress(UpdatedAddress);

            return RedirectToAction("CustomerPage");
        }

        /// <summary>
        /// GET: CreateAddressBook layout
        /// </summary>
        /// <param name="AddressID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult CreateAddressBook(Guid CustomerID)
        {
            return View();
        }

        /// <summary>
        /// POST: Create Address in Dialog
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="input"></param>
        /// <param name="AddressID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult CreateAddressBook(Guid CustomerID, AddressViewModel input)
        {
            Address CreateAddress = new Address();
            CreateAddress.CustomerId = input.CustomerId;
            CreateAddress.AddressId = Guid.NewGuid();
            CreateAddress.Name = input.CustomerAddressName;
            CreateAddress.Address1 = input.CustomerAddress;
            CreateAddress.State = input.CustomerState;
            CreateAddress.City = input.CustomerCity;
            CreateAddress.Postcode = input.CustomerPostcode;
            CreateAddress.Country = input.CustomerCountry;
            CreateAddress.Contact = input.CustomerCountry;

            CustomerRepository.CreateShippingAddress(CreateAddress);

            return RedirectToAction("CustomerPage");
        }

        [HttpPost]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult DeleteAddressBook(Guid AddressID)
        {
            CustomerRepository.DeleteAddress(AddressID);
            return RedirectToAction("CustomerPage");
        }
        #endregion

        #region My Cart
        /// <summary>
        /// GET: My Cart tab
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult MyCart(Guid CustomerID)
        {
            CartViewModel output = new CartViewModel();

            if (CustomerID != null)
            {
                var CartList = CartRepository.GetCartList(CustomerID);

                output.CartItems = CartList
                    .Join(ProductRepository.Products, x => x.ProductId, y => y.ProductId,
                    (x, y) =>
                        new ProductInfo()
                        {
                            CustomerID = CustomerID,
                            CartID = x.CartId,
                            CartListID = x.CartListId,
                            ProductID = y.ProductId,
                            ProductCategory = y.ProductCategory,
                            ProductCode = y.ProductCode,
                            ProductDescription = y.ProductDescription,
                            ProductName = y.ProductName,
                            ProductRetailPrice = y.ProductRetailPrice,
                            ProductQuantity = x.CartListTotalQuantity,
                            ProductImageByte = y.Images.Select(z => z.ProductImageSource).FirstOrDefault(),
                            ProductImageType = y.Images.Select(z => z.ProductImageType).FirstOrDefault(),
                            ProductStatus = y.ProductStatus
                        }
                    )
                    .ToList();
            }
            return View(output);
        }
        #endregion

        #region Order History
        /// <summary>
        /// GET: Order History tab
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult OrderHistory(Guid CustomerID)
        {
            var OrderHistoryRecords = CustomerRepository.GetOrderHistoryByCustomerID(CustomerID);
            List<OrderHistory> orderhistorylist = new List<OrderHistory>();
            OrderHistoryViewModel orderhistoryvm = new OrderHistoryViewModel();
            foreach (var item in OrderHistoryRecords)
            {
                OrderHistory orderhistory = new OrderHistory();
                orderhistory.OrderNo = item.OrderID;
                orderhistory.OrderDate = (item.OrderCreatedOn != null ? item.OrderCreatedOn.Value.ToString("dd/MM/yyyy") : "[n/a]");
                orderhistory.Title = item.ProductName;
                orderhistory.ProductImageByte = item.ProductImageSource;
                orderhistory.ProductImageType = item.ProductImageType;
                orderhistory.OrderQuantity = item.CartListTotalQuantity;
                orderhistory.Amount = decimal.Round(item.CartListTotalPrice, 2, MidpointRounding.AwayFromZero);
                orderhistory.OrderStatus = Status.OrderStatusConverter(item.OrderStatus);
                orderhistorylist.Add(orderhistory);
            }
            orderhistoryvm.OrderRecords = orderhistorylist;
            return View(orderhistoryvm);
        }

        #endregion

        #region Specialization
        /// <summary>
        /// GET: Specialization tab
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult Specialization(Guid CustomerID)
        {
            var Tailor = CustomerRepository.GetTailorByCustomerID(CustomerID);
            TailorViewModel output = new TailorViewModel();
            output.TailorID = Tailor.TailorID;
            output.CustomerID = Tailor.CustomerID;
            output.Specialization = Tailor.Specialization;
            output.AverageRating = Tailor.AverageRating;
            output.AverageElapsedDay = Tailor.AverageElapsedDay;            
            output.OrderInHand = Tailor.OrderInHand;
            output.OrderDone = Tailor.OrderDone;
            return View(output);
        }

        /// <summary>
        /// GET: Update Specialization tab page
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult UpdateSpecialization(Guid CustomerID)
        {
            return View();
        }

        #endregion

        #region Task History
        /// <summary>
        /// GET: Task History tab
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult TaskHistory(Guid CustomerID)
        {
            Tailor tailor = CustomerRepository.GetTailorByCustomerID(CustomerID);
            Guid TailorID = tailor.TailorID;
            var TaskHistoryRecords = CustomerRepository.GetTaskHistoryByTailorID(TailorID);
            List<TaskHistory> taskhistorylist = new List<TaskHistory>();
            TaskHistoryViewModel taskhistoryvm = new TaskHistoryViewModel();
            foreach(var item in TaskHistoryRecords)
            {
                TaskHistory taskhistory = new TaskHistory();
                taskhistory.OrderNo = item.OrderID;
                taskhistory.OrderDate = (item.OrderCreatedOn != null ? item.OrderCreatedOn.Value.ToString("dd/MM/yyyy") : "[n/a]");
                taskhistory.Deadline = (DateTime)item.OrderDeadline;
                taskhistory.Title = item.ProductName;
                taskhistory.ProductImageByte = item.ProductImageSource;
                taskhistory.ProductImageType = item.ProductImageType;
                taskhistory.OrderQuantity = item.CartListTotalQuantity;
                taskhistory.OrderStatus = item.OrderStatus;
                taskhistorylist.Add(taskhistory);
            }
            taskhistoryvm.TaskRecords = taskhistorylist;
            taskhistoryvm.TailorID = TailorID;
            return View(taskhistoryvm);
        }

        #endregion

        #endregion

        /// <summary>
        /// Authentication username and role 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Customer()
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
            return PartialView();
        }

        /// <summary>
        /// Verify Customer Account Email 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ActionResult ConfirmEmail(Guid CustomerID, String token)
        {
            //TODO: Call backend verify email
            if (CustomerRepository.VerifyAccount(CustomerID, token))
            {
                ViewBag.Message = "Your account had been verified. Please log in again to start shopping.";
                ViewBag.Result = true;
            }
            else
            {
                ViewBag.Message = "The token is expired. Please click on button below to get a new token<br/>";
                ViewBag.Result = false;
            }
            return View(CustomerID);
        }

        [HttpPost]
        public async Task<ActionResult> RequestConfirmationEmail(Guid CustomerID)
        {
            Customer customer = CustomerRepository.GetCustomerByID(CustomerID);
            string code = await CustomerRepository.GenerateEmailConfirmationTokenAsync(customer);
            var callbackUrl = Url.Action("ConfirmEmail", "Customer",
                    new { CustomerID = customer.CustomerID, token = code },
                    Request.Url.Scheme).Replace("amp;", "");
            provider.SendConfirmationEmail(customer, "Confirm your account",
                                   "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            ViewBag.Message = "Another verification email had been sent. Please verify your account in 30 minutes from now.";
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> RequestConfirmationEmailGet(Guid CustomerID)
        {
            Customer customer = CustomerRepository.GetCustomerByID(CustomerID);
            string code = await CustomerRepository.GenerateEmailConfirmationTokenAsync(customer);
            var callbackUrl = Url.Action("ConfirmEmail", "Customer",
                    new { CustomerID = customer.CustomerID, token = code },
                    Request.Url.Scheme).Replace("amp;", "");
            provider.SendConfirmationEmail(customer, "Confirm your account",
                                   "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            ViewBag.Message = "Another verification email had been sent. Please verify your account in 30 minutes from now.";
            return View();
        }

        [HttpPost]
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public ActionResult CheckOutCart(string customerID, string cartID, int pref)
        {
            Guid CustomerID = new Guid(customerID);
            Guid CartID = new Guid(cartID);
            List<SucceedOrder> OrderList = new List<SucceedOrder>();

            //Place Cart Items to Orders
            using (SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;"))
            {
                using (SqlCommand cmd = new SqlCommand("CheckOutCart", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SucceedCartID", SqlDbType.UniqueIdentifier).Value = CartID;
                    con.Open();
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //read row by row
                            while (reader.Read())
                            {
                                //get data column by column
                                var OrderItem = new SucceedOrder();
                                OrderItem.orderID = reader.GetGuid(0);
                                OrderItem.succeedcartID = reader.GetGuid(1);
                                OrderItem.cartItemID = reader.GetGuid(2);
                                OrderItem.productID = reader.GetGuid(3);
                                OrderItem.templateID = reader.GetGuid(4);
                                OrderItem.orderStatus = reader.GetInt32(5);
                                OrderItem.cartStatus = reader.GetInt32(6);
                                OrderItem.orderDeadline = reader.GetDateTime(7);
                                OrderItem.totalQuantity = reader.GetInt32(8);
                                //OrderItem.orderBeginTime = DateTime.Now;
                                //OrderItem.orderFinishedTime = reader.GetDateTime(9);
                                OrderList.Add(OrderItem);
                            }
                        }
                    }
                }
            }

            //Get each cart item going to assign
            foreach (var item in OrderList)
            {               
                Guid designerID = CustomerRepository.GetDesignerID(item.productID);
                int rewardPoints = item.totalQuantity * 5;
                CustomerRepository.UpdateCustomerPoint(designerID, rewardPoints);
                List<Specialize> specializedTailor = new List<Specialize>();
                List<Guid> experiencedTailorIDs = new List<Guid>();
                Guid dedicatedTailorID = new Guid();
                bool trialtailorAssigned = false;
                //var succeedCartItems = OrderRepository.GetSucceedCartItems(item.cartItemID);
                specializedTailor = OrderRepository.GetSpecializedTailor(item.templateID);
                //first loop finding trial tailor
                foreach (var tailor in specializedTailor)
                {
                    var taskhistory = OrderRepository.GetTailorOrderAssignment(tailor.TailorID);
                    if (taskhistory.Count < 3 && !trialtailorAssigned)
                    {
                        dedicatedTailorID = tailor.TailorID;
                        trialtailorAssigned = true;
                        break;
                    }
                    //else
                    //{
                    //    if (taskhistory.Count >= 3)
                    //    {
                    //        experiencedTailorIDs.Add(tailor.TailorID);
                    //    }
                    //}
                }

                //if no trial tailor exist
                if (!trialtailorAssigned)
                {
                    if (pref == 1)
                    {
                        List<vw_ExpTailorAssignment> rank1Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 1);
                        if (rank1Tailor.Count > 0)
                        {
                            //no monolithic, performance issue no need loop through all tailor
                            Random rand = new Random();
                            int randomfactor = rand.Next(rank1Tailor.Count);
                            int tailorNo = 0;
                            foreach (var chosentailor in rank1Tailor)
                            {
                                if (tailorNo == randomfactor)
                                {
                                    dedicatedTailorID = chosentailor.TailorID;
                                    break;
                                }
                                else
                                {
                                    tailorNo++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            List<vw_ExpTailorAssignment> rank2Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 2);
                            if (rank2Tailor.Count > 0)
                            {
                                Random rand = new Random();
                                int randomfactor = rand.Next(rank2Tailor.Count);
                                int tailorNo = 0;
                                foreach (var chosentailor in rank2Tailor)
                                {
                                    if (tailorNo == randomfactor)
                                    {
                                        dedicatedTailorID = chosentailor.TailorID;
                                        break;
                                    }
                                    else
                                    {
                                        tailorNo++;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                List<vw_ExpTailorAssignment> rank3Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 3);
                                if (rank3Tailor.Count > 0)
                                {
                                    Random rand = new Random();
                                    int randomfactor = rand.Next(rank3Tailor.Count);
                                    int tailorNo = 0;
                                    foreach (var chosentailor in rank3Tailor)
                                    {
                                        if (tailorNo == randomfactor)
                                        {
                                            dedicatedTailorID = chosentailor.TailorID;
                                            break;
                                        }
                                        else
                                        {
                                            tailorNo++;
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    List<vw_ExpTailorAssignment> rank4Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 4);
                                    if (rank4Tailor.Count > 0)
                                    {
                                        Random rand = new Random();
                                        int randomfactor = rand.Next(rank4Tailor.Count);
                                        int tailorNo = 0;
                                        foreach (var chosentailor in rank4Tailor)
                                        {
                                            if (tailorNo == randomfactor)
                                            {
                                                dedicatedTailorID = chosentailor.TailorID;
                                                break;
                                            }
                                            else
                                            {
                                                tailorNo++;
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<vw_ExpTailorAssignment> rank5Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 5);
                                        if (rank5Tailor.Count > 0)
                                        {
                                            Random rand = new Random();
                                            int randomfactor = rand.Next(rank5Tailor.Count);
                                            int tailorNo = 0;
                                            foreach (var chosentailor in rank5Tailor)
                                            {
                                                if (tailorNo == randomfactor)
                                                {
                                                    dedicatedTailorID = chosentailor.TailorID;
                                                    break;
                                                }
                                                else
                                                {
                                                    tailorNo++;
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            List<vw_ExpTailorAssignment> rank6Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 6);
                                            if (rank6Tailor.Count > 0)
                                            {
                                                Random rand = new Random();
                                                int randomfactor = rand.Next(rank6Tailor.Count);
                                                int tailorNo = 0;
                                                foreach (var chosentailor in rank6Tailor)
                                                {
                                                    if (tailorNo == randomfactor)
                                                    {
                                                        dedicatedTailorID = chosentailor.TailorID;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        tailorNo++;
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                List<vw_ExpTailorAssignment> rank7Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 7);
                                                if (rank7Tailor.Count > 0)
                                                {
                                                    Random rand = new Random();
                                                    int randomfactor = rand.Next(rank7Tailor.Count);
                                                    int tailorNo = 0;
                                                    foreach (var chosentailor in rank7Tailor)
                                                    {
                                                        if (tailorNo == randomfactor)
                                                        {
                                                            dedicatedTailorID = chosentailor.TailorID;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            tailorNo++;
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    List<vw_ExpTailorAssignment> rank8Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 8);
                                                    if (rank8Tailor.Count > 0)
                                                    {
                                                        Random rand = new Random();
                                                        int randomfactor = rand.Next(rank8Tailor.Count);
                                                        int tailorNo = 0;
                                                        foreach (var chosentailor in rank8Tailor)
                                                        {
                                                            if (tailorNo == randomfactor)
                                                            {
                                                                dedicatedTailorID = chosentailor.TailorID;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                tailorNo++;
                                                                continue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pref == 2)
                    {
                        List<vw_ExpTailorAssignment> rank1Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 1);
                        if (rank1Tailor.Count > 0)
                        {
                            //no monolithic, performance issue no need loop through all tailor
                            Random rand = new Random();
                            int randomfactor = rand.Next(rank1Tailor.Count);
                            int tailorNo = 0;
                            foreach (var chosentailor in rank1Tailor)
                            {
                                if (tailorNo == randomfactor)
                                {
                                    dedicatedTailorID = chosentailor.TailorID;
                                    break;
                                }
                                else
                                {
                                    tailorNo++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            List<vw_ExpTailorAssignment> rank2Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 5);
                            if (rank2Tailor.Count > 0)
                            {
                                Random rand = new Random();
                                int randomfactor = rand.Next(rank2Tailor.Count);
                                int tailorNo = 0;
                                foreach (var chosentailor in rank2Tailor)
                                {
                                    if (tailorNo == randomfactor)
                                    {
                                        dedicatedTailorID = chosentailor.TailorID;
                                        break;
                                    }
                                    else
                                    {
                                        tailorNo++;
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                List<vw_ExpTailorAssignment> rank3Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 2);
                                if (rank3Tailor.Count > 0)
                                {
                                    Random rand = new Random();
                                    int randomfactor = rand.Next(rank3Tailor.Count);
                                    int tailorNo = 0;
                                    foreach (var chosentailor in rank3Tailor)
                                    {
                                        if (tailorNo == randomfactor)
                                        {
                                            dedicatedTailorID = chosentailor.TailorID;
                                            break;
                                        }
                                        else
                                        {
                                            tailorNo++;
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    List<vw_ExpTailorAssignment> rank4Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 6);
                                    if (rank4Tailor.Count > 0)
                                    {
                                        Random rand = new Random();
                                        int randomfactor = rand.Next(rank4Tailor.Count);
                                        int tailorNo = 0;
                                        foreach (var chosentailor in rank4Tailor)
                                        {
                                            if (tailorNo == randomfactor)
                                            {
                                                dedicatedTailorID = chosentailor.TailorID;
                                                break;
                                            }
                                            else
                                            {
                                                tailorNo++;
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<vw_ExpTailorAssignment> rank5Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 3);
                                        if (rank5Tailor.Count > 0)
                                        {
                                            Random rand = new Random();
                                            int randomfactor = rand.Next(rank5Tailor.Count);
                                            int tailorNo = 0;
                                            foreach (var chosentailor in rank5Tailor)
                                            {
                                                if (tailorNo == randomfactor)
                                                {
                                                    dedicatedTailorID = chosentailor.TailorID;
                                                    break;
                                                }
                                                else
                                                {
                                                    tailorNo++;
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            List<vw_ExpTailorAssignment> rank6Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 7);
                                            if (rank6Tailor.Count > 0)
                                            {
                                                Random rand = new Random();
                                                int randomfactor = rand.Next(rank6Tailor.Count);
                                                int tailorNo = 0;
                                                foreach (var chosentailor in rank6Tailor)
                                                {
                                                    if (tailorNo == randomfactor)
                                                    {
                                                        dedicatedTailorID = chosentailor.TailorID;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        tailorNo++;
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                List<vw_ExpTailorAssignment> rank7Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 4);
                                                if (rank7Tailor.Count > 0)
                                                {
                                                    Random rand = new Random();
                                                    int randomfactor = rand.Next(rank7Tailor.Count);
                                                    int tailorNo = 0;
                                                    foreach (var chosentailor in rank7Tailor)
                                                    {
                                                        if (tailorNo == randomfactor)
                                                        {
                                                            dedicatedTailorID = chosentailor.TailorID;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            tailorNo++;
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    List<vw_ExpTailorAssignment> rank8Tailor = OrderRepository.GetExperiencedTailor(item.templateID, 8);
                                                    if (rank8Tailor.Count > 0)
                                                    {
                                                        Random rand = new Random();
                                                        int randomfactor = rand.Next(rank8Tailor.Count);
                                                        int tailorNo = 0;
                                                        foreach (var chosentailor in rank8Tailor)
                                                        {
                                                            if (tailorNo == randomfactor)
                                                            {
                                                                dedicatedTailorID = chosentailor.TailorID;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                tailorNo++;
                                                                continue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else 
                    {
                        
                    }
                    //foreach (var experiencedtailor in specializedTailor)
                    //{

                    //}
                }

                //OrderRepository.AssignToTailor(dedicatedTailorID, item.orderID, item.totalQuantity);

                //Place Cart Items to Orders
                using (SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;"))
                {
                    using (SqlCommand cmd = new SqlCommand("AssignOrderToTailor", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SelectedTailorID", SqlDbType.UniqueIdentifier).Value = dedicatedTailorID;
                        cmd.Parameters.Add("@AssignedOrderID", SqlDbType.UniqueIdentifier).Value = item.orderID;
                        cmd.Parameters.Add("@OrderQuantity", SqlDbType.Int).Value = item.totalQuantity;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                //read row by row
                                //while (reader.Read())
                                //{
                                //    //get data column by column
                                //    var OrderItem = new SucceedOrder();
                                //    OrderItem.orderID = reader.GetGuid(0);
                                //    OrderItem.succeedcartID = reader.GetGuid(1);
                                //    OrderItem.cartItemID = reader.GetGuid(2);
                                //    OrderItem.productID = reader.GetGuid(3);
                                //    OrderItem.templateID = reader.GetGuid(4);
                                //    OrderItem.orderStatus = reader.GetInt32(5);
                                //    OrderItem.cartStatus = reader.GetInt32(6);
                                //    OrderItem.orderDeadline = reader.GetDateTime(7);
                                //    OrderItem.totalQuantity = reader.GetInt32(8);
                                //    //OrderItem.orderBeginTime = DateTime.Now;
                                //    //OrderItem.orderFinishedTime = reader.GetDateTime(9);
                                //    OrderList.Add(OrderItem);
                                //}
                            }
                        }
                    }
                }
            }
            



            //CartRepository.CheckOutCart(cartID);

            //get the succeed cart items
            //List<CartItem> succeedCartItems = CartRepository.GetSucceedCartItems(CustomerID);

            //store in order
            //foreach (var cartItem in succeedCartItems)
            //{
            //    Order newOrder = new Order();
            //    //newOrder.OrderID = Guid.NewGuid();
            //    newOrder.CartListId = cartItem.CartListId;
            //    //newOrder.OrderStatus = Status.ORDER_PENDING;
            //    CustomerRepository.AddNewOrder(newOrder);
            //}

            //Assign to Tailor
            return View();
        }

        /// <summary>
        /// Update Order Status in Orders
        /// </summary>
        /// <param name="OrderID"></param>
        [Authorize(Roles = RoleAssignment.ROLE_TAILOR_NAME)]
        public void CompleteOrder(Nullable<Guid> OrderID, Nullable<Guid> TailorID, Nullable<int> OrderQuantity)
        {
            using (SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;"))
            {
                using (SqlCommand cmd = new SqlCommand("TailorCompleteOrder", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompletedOrderID", SqlDbType.UniqueIdentifier).Value = OrderID; 
                    cmd.Parameters.Add("@CompletedTailorID", SqlDbType.UniqueIdentifier).Value = TailorID;
                    cmd.Parameters.Add("@CompletedOrderQuantity", SqlDbType.Int).Value = OrderQuantity;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //read row by row
                            while (reader.Read())
                            {
                                //get data column by column
                            }
                        }
                    }
                }
            }
            //OrderRepository.CompleteOrder(OrderID, TailorID, OrderQuantity);
            //return View();
        }

        /// <summary>
        /// Submit Review for Orders
        /// </summary>
        /// <param name="OrderID"></param>
        [AuthorizeRoles(RoleAssignment.ROLE_MEMBER_NAME, RoleAssignment.ROLE_TAILOR_NAME)]
        public void Review(Nullable<Guid> OrderID, string Comment, int Rating)
        {
            Guid OID = (Guid)OrderID;
            vw_OrderHistory reviewedOrder = CustomerRepository.GetOrderHistoryByOrderID(OID);
            Guid reviewedCustomerID = reviewedOrder.CustomerID;
            Customer reviewedCustomer = CustomerRepository.GetCustomerByID(reviewedCustomerID);
            String[] substrings = Comment.Split(new Char[] { ',', '\n' , '.' , ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Hashtable afinn = affinlist.htAfinn;
            decimal sentiment_score = 0;
            decimal overall_sentiment_score = 0;
            decimal positive_pts = 0;
            decimal negative_pts = 0;
            decimal average_positive = 0;
            decimal average_negative = 0;
            List<string> positive_words = new List<string>();
            List<string> negative_words = new List<string>();
            List<string> neutral_words = new List<string>();
            bool consecutive = false;
            string temp = null;
            

            foreach (var substring in substrings)
            {
                string word = substring.ToLower();
                if (word == "not" || word == "no")
                {
                    temp = word;
                    consecutive = true;
                    continue;
                }
                if (consecutive)
                {
                    word = temp + ' ' + word;
                    consecutive = false;
                    temp = null;
                }
                int n;
                bool isNumeric = int.TryParse(word, out n);
                if (!isNumeric)
                {
                    if (afinn.ContainsKey(word))
                    {                        
                        int score = 0;
                        if (Int32.TryParse(afinn[word].ToString(), out score))
                        {
                            if (score > 0)
                            {
                                positive_words.Add(word);
                                positive_pts += score;                                
                            }
                            else if (score < 0)
                            {
                                negative_words.Add(word);
                                negative_pts += score;                                
                            }
                            else
                            {
                                neutral_words.Add(word);
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("String could not be parsed.");
                        }
                    }
                }
            }

            string keywords = null;
            if (positive_words.Count != 0)
            {
                average_positive = positive_pts / positive_words.Count;
                keywords = string.Join(", ", positive_words);
            }
            if (negative_words.Count != 0)
            {
                average_negative = negative_pts / negative_words.Count;
                if(keywords != null)
                    keywords += ", ";
                keywords += string.Join(", ", negative_words);
            }
            sentiment_score = average_positive + average_negative;

            int polar = 0;
            if (sentiment_score >= 0)
                polar = 1;
            else
                polar = 0;

            overall_sentiment_score = (sentiment_score + Rating) / 2;
            
            using (SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;"))
            {
                using (SqlCommand cmd = new SqlCommand("CustomerRateProduct", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RatedOrderID", SqlDbType.UniqueIdentifier).Value = OrderID;
                    cmd.Parameters.Add("@RatingValue", SqlDbType.Decimal).Value = Math.Round(overall_sentiment_score, 2);
                    cmd.Parameters.Add("@ReviewComments", SqlDbType.NVarChar).Value = Comment;
                    cmd.Parameters.Add("@ReviewKeywords", SqlDbType.NVarChar).Value = keywords;
                    cmd.Parameters.Add("@Polarity", SqlDbType.Int).Value = polar;
                    cmd.Parameters.Add("@RatedCustomer", SqlDbType.NVarChar).Value = reviewedCustomer.CustomerFirstName + ' ' + reviewedCustomer.CustomerLastName;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            //read row by row
                            while (reader.Read())
                            {
                                //get data column by column
                            }
                        }
                    }
                }
            }

            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");
            //afinn.Add("abandon", "-2");

            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //string startupPath1 = Environment.CurrentDirectory;


            //try
            //{
            //    //Set the current directory.
            //    //Directory.SetCurrentDirectory("C:/Users/Jex/Documents/Visual Studio 2013/Projects/DIYCommerce/ECWebApp.WebUI/TextFile");
            //    foreach (var substring in substrings)
            //    {
            //        string word = substring.ToLower();
            //        if (word == "not" || word == "no")
            //        {
            //            temp = word;
            //            consecutive = true;
            //            continue;
            //        }
            //        if (consecutive)
            //        {
            //            word = temp + ' ' + word;
            //            consecutive = false;
            //            temp = null;
            //        }
            //        int n;
            //        bool isNumeric = int.TryParse(word, out n);
            //        if (!isNumeric)
            //        {
            //            using (var mappedFile1 = MemoryMappedFile.CreateFromFile("C:/Users/Jex/Documents/Visual Studio 2013/Projects/DIYCommerce/ECWebApp.WebUI/TextFile/AFINN-111.txt"))
            //            {
            //                using (Stream mmStream = mappedFile1.CreateViewStream())
            //                {
            //                    using (StreamReader sr = new StreamReader(mmStream, ASCIIEncoding.ASCII))
            //                    {
            //                        while (!sr.EndOfStream)
            //                        {
            //                            var line = sr.ReadLine();
            //                            var lineWords = line.Split('\t');
            //                            if (word == lineWords[0])
            //                            {
            //                                int score;
            //                                if (Int32.TryParse(lineWords[1], out score))
            //                                {
            //                                    if (score > 0)
            //                                    {
            //                                        positive_words.Add(lineWords[0]);
            //                                        positive_pts += score;
            //                                        break;
            //                                    }
            //                                    else if (score < 0)
            //                                    {
            //                                        negative_words.Add(lineWords[0]);
            //                                        negative_pts += score;
            //                                        break;
            //                                    }
            //                                    else
            //                                    {
            //                                        neutral_words.Add(lineWords[0]);
            //                                        break;
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine("String could not be parsed.");
            //                                }
                                            
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    string keywords = null;
            //    if (positive_words.Count != 0)
            //    {
            //        average_positive = positive_pts / positive_words.Count;
            //        keywords = string.Join(", ", positive_words);
            //    }
            //    if (negative_words.Count != 0)
            //    {
            //        average_negative = negative_pts / negative_words.Count;
            //        keywords += string.Join(", ", negative_words);
            //    }
            //    sentiment_score = average_positive + average_negative;
            //    overall_sentiment_score = (sentiment_score + Rating)/2;

                
            //    using (SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;"))
            //    {
            //        using (SqlCommand cmd = new SqlCommand("CustomerRateProduct", con))
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add("@RatedOrderID", SqlDbType.UniqueIdentifier).Value = OrderID;
            //            cmd.Parameters.Add("@RatingValue", SqlDbType.Decimal).Value = Math.Round(overall_sentiment_score, 2);
            //            cmd.Parameters.Add("@ReviewKeywords", SqlDbType.NVarChar).Value = keywords;
            //            con.Open();

            //            using (SqlDataReader reader = cmd.ExecuteReader())
            //            {
            //                if (reader.HasRows)
            //                {
            //                    //read row by row
            //                    while (reader.Read())
            //                    {
            //                        //get data column by column
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (DirectoryNotFoundException e)
            //{
            //    Console.WriteLine("The specified directory does not exist. {0}", e);
            //}
            
            //OrderRepository.RatingOrder(OrderID, Rating);
            //return View();
        }
    }
}