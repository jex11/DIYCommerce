using System.Collections.Generic;

using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using System;
using System.Linq;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Postal;
using System.Data.SqlClient;


namespace ECWebApp.Domain.Concrete
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();

        public IEnumerable<Customer> Customers
        {
            get { return context.Customers.Where(x => x.CustomerStatus == Constant.Status.CUSTOMER_ACTIVE); }
        }

        public IEnumerable<Customer> DesignCustomers
        {
            get { return context.Customers.Where(x => x.CustomerStatus == Constant.Status.CUSTOMER_ACTIVE && x.CustomerCode == 1); }
        }
        #region Authentication
            /// <summary>
            /// Authenticate user
            /// </summary>
            /// <param name="Result"></param>
            /// <param name="RememberMe"></param>
            /// <returns></returns>
            public bool Authenticate(Customer Result, bool RememberMe)
        {
            Guid UserID = context.Customers
                .Where(x => x.CustomerEmail.Equals(Result.CustomerEmail) && x.CustomerPassword.Equals(Result.CustomerPassword))
                .Select(x => x.CustomerID).FirstOrDefault();

            if (UserID == Guid.Empty)
            {
                return false;
            }

            String AuthToken = string.Join("", MD5.Create()
                                .ComputeHash(Encoding.ASCII.GetBytes(
                                UserID.ToString() +
                                Result.CustomerEmail +
                                Result.CustomerPassword +
                                Result.CustomerCreatedOn.ToString()))
                                .Select(s => s.ToString("x2")));

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1,                                     // ticket version
                                UserID.ToString(),                     // authenticated username
                                DateTime.Now,                          // issueDate
                                DateTime.Now.AddMinutes(30),           // expiryDate
                                RememberMe,                            // true to persist across browser sessions
                                AuthToken,                            // Hash Value for User Token
                                FormsAuthentication.FormsCookiePath);  // the path for the cookie

            // Encrypt the ticket using the machine key
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Add the cookie to the request to save it
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            HttpContext.Current.Session[FormsAuthentication.FormsCookieName] = encryptedTicket;
            HttpContext.Current.Response.SetCookie(cookie);

            LastAccess Session = new LastAccess();
            Session.CustomerId = UserID;
            Session.LastAccessId = Guid.NewGuid();
            Session.LastAccessDevice = AuthToken;
            Session.LastAccessCreatedBy = Result.CustomerID.ToString();
            Session.LastAccessCreatedOn = DateTime.Now;
            //context.LastAccesses.Add(Session);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
            
            /// <summary>
            /// Generate Email Confirmation Token
            /// </summary>
            /// <param name="Result"></param>
            /// <returns></returns>
            public Task<string> GenerateEmailConfirmationTokenAsync(Customer Result)
            {
                return Task.Run<string>(() => GenerateEmailConfirmationToken(Result));
            }
            
            /// <summary>
            /// Generate Email Confirmation Token Method
            /// </summary>
            /// <param name="Result"></param>
            /// <returns></returns>
            private string GenerateEmailConfirmationToken(Customer Result)
            {
                Guid CustomerID = context.Customers
                .Where(x => x.CustomerEmail.Equals(Result.CustomerEmail) && x.CustomerPassword.Equals(Result.CustomerPassword))
                .Select(x => x.CustomerID).FirstOrDefault();
                String token =  string.Join("", MD5.Create()
                                .ComputeHash(Encoding.ASCII.GetBytes(
                                CustomerID +
                                Result.CustomerEmail +
                                Result.CustomerPassword +
                                Result.CustomerCreatedOn.ToString()))
                                .Select(s => s.ToString("x2")));
                EmailConfirm NewConfirmationToken = new EmailConfirm();
                NewConfirmationToken.ConfirmationCode = token;
                NewConfirmationToken.ConfirmationStatus = Status.CUSTOMER_CONFIRMATION_TOKEN_ACTIVE;
                NewConfirmationToken.CustomerEmail = Result.CustomerEmail;
                NewConfirmationToken.CustomerID = Result.CustomerID;
                NewConfirmationToken.EmailConfirmCreatedOn = DateTime.Now;
                NewConfirmationToken.EmailConfirmEndOn = DateTime.Now.AddMinutes(30);
                NewConfirmationToken.EmailConfirmId = Guid.NewGuid();
                context.EmailConfirms.Add(NewConfirmationToken);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
                return token;
            }
            
            /// <summary>
            /// Sign up new customer
            /// </summary>
            /// <param name="customer"></param>
            /// <returns></returns>
            public bool SignUp(Customer customer)
        {
            RoleAssign role = new RoleAssign();
            role.CustomerId = customer.CustomerID;
            role.RoleAssignId = Guid.NewGuid();
            role.RoleId = context.Roles
                .Where(x => x.RoleName.Equals("Member") && x.RoleStatus.Equals(Status.CUSTOMER_ROLE_ACTIVE))
                .Select(x => x.RoleId)
                .FirstOrDefault();
            role.RoleAssignCreatedBy = customer.CustomerID.ToString();
            role.RoleAssignCreatedOn = DateTime.Now;
                //TODO: i think this version i give u din have.. 
                // add a check here on whether this line return u anything anot . i can just add the breakpoint here? will it like a wpf apps 
                //yes..saw that?  ya it will trigger when i execute the function, but where is the page?                

            try
            {
                context.Customers.Add(customer);
                context.RoleAssigns.Add(role);
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return false;
            }
        }

            /// <summary>
            /// Validate email existence
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public bool ValidateEmail(String email)
            {
                List<string> result = context.Customers
                    .Where(x => x.CustomerEmail.Equals(email))
                    .Select(x => x.CustomerEmail).ToList();
                if (result.Count.Equals(0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            /// <summary>
            /// Verify Account after signup
            /// </summary>
            /// <param name="CustomerID"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public bool VerifyAccount(Guid CustomerID, String token)
            {
                EmailConfirm ConfirmationToken = context.EmailConfirms
                                                    .Where(x => x.ConfirmationCode.Equals(token) && x.ConfirmationStatus == Status.CUSTOMER_CONFIRMATION_TOKEN_ACTIVE)
                                                    //.Select(x => x.EmailConfirmEndOn)
                                                    .FirstOrDefault();
                if (ConfirmationToken == null)
                {
                    return false;
                }
                if (DateTime.Now <= ConfirmationToken.EmailConfirmEndOn)
                {
                    Customer ConfirmedAccount = context.Customers
                                                       .Where(x => x.CustomerID.Equals(CustomerID) && x.CustomerStatus == Status.CUSTOMER_PENDING)
                                                       .FirstOrDefault();
                    ConfirmedAccount.CustomerStatus = Status.CUSTOMER_ACTIVE;
                    try
                    {
                        context.SaveChanges();
                        return true;
                    }
                    catch (DbEntityValidationException e)
                    {
                        Console.WriteLine(e);
                    }

                }
                return false;
            }
        #endregion

        #region Customer Details
            /// <summary>
            /// Get customer by using customer id
            /// </summary>
            /// <param name="CustomerID"></param>
            /// <returns></returns>
            public Customer GetCustomerByID(Guid CustomerID)
            {
                return context.Customers
                    .Where(x => x.CustomerID.Equals(CustomerID))
                    .FirstOrDefault();

            }

            /// <summary>
            /// Get Customer id through customer details 
            /// </summary>
            /// <param name="customer"></param>
            /// <returns></returns>
            public Guid GetCustomerID(Customer customer)
            {
                Guid CustomerID = context.Customers
                    .Where(x => x.CustomerEmail.Equals(customer.CustomerEmail) && x.CustomerPassword.Equals(customer.CustomerPassword))
                    .Select(x => x.CustomerID).FirstOrDefault(); 
                return CustomerID;
            }
            

            /// <summary>
            /// Update customer username
            /// </summary>
            /// <param name="NewRecord"></param>
            public void UpdateCustomerUsername(Customer NewRecord)
            {
                Customer OldRecord = context.Customers.Where(x => x.CustomerID.Equals(NewRecord.CustomerID)).FirstOrDefault();
                OldRecord.CustomerFirstName = NewRecord.CustomerFirstName;
                OldRecord.CustomerLastName = NewRecord.CustomerLastName;
                OldRecord.CustomerUpdatedOn = DateTime.Now;
                OldRecord.CustomerUpdatedBy = NewRecord.CustomerID.ToString();
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }

            /// <summary>
            /// Update Customer Contact Number
            /// </summary>
            /// <param name="NewRecord"></param>
            public void UpdateCustomerContactNo(Customer NewRecord)
            {
                try
                {
                    Customer OldRecord = context.Customers.Where(x => x.CustomerID.Equals(NewRecord.CustomerID)).FirstOrDefault();
                    OldRecord.CustomerContact = NewRecord.CustomerContact;
                    OldRecord.CustomerUpdatedOn = DateTime.Now;
                    OldRecord.CustomerUpdatedBy = NewRecord.CustomerID.ToString();
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }

            /// <summary>
            /// Update Customer Email
            /// </summary>
            /// <param name="NewRecord"></param>
            public void UpdateCustomerEmail(Customer NewRecord)
            {
                try
                {
                    Customer OldRecord = context.Customers.Where(x => x.CustomerID.Equals(NewRecord.CustomerID)).FirstOrDefault();
                    OldRecord.CustomerEmail = NewRecord.CustomerEmail;
                    OldRecord.CustomerUpdatedOn = DateTime.Now;
                    OldRecord.CustomerUpdatedBy = NewRecord.CustomerID.ToString();

                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }
            
            /// <summary>
            /// Update Customer Password
            /// </summary>
            /// <param name="NewRecord"></param>
            public void UpdateCustomerPassword(Customer NewRecord)
            {
                try
                {
                    Customer OldRecord = context.Customers.Where(x => x.CustomerID.Equals(NewRecord.CustomerID)).FirstOrDefault();
                    OldRecord.CustomerPassword = NewRecord.CustomerPassword;
                    OldRecord.CustomerUpdatedOn = DateTime.Now;
                    OldRecord.CustomerUpdatedBy = NewRecord.CustomerID.ToString();
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }


            public void UpdateCustomerProfileImage(Customer NewRecord)
            {
                try
                {
                    Customer OldRecord = context.Customers.Where(x => x.CustomerID.Equals(NewRecord.CustomerID)).FirstOrDefault();
                    OldRecord.CustomerImg = NewRecord.CustomerImg;
                    OldRecord.CustomerImgType = NewRecord.CustomerImgType;
                    OldRecord.CustomerUpdatedOn = DateTime.Now;
                    OldRecord.CustomerUpdatedBy = NewRecord.CustomerID.ToString();
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }

            public Guid GetDesignerID(Guid OrderedProductID)
            {
                Guid DesignerID = context.CustomProducts
                    .Where(x => x.ProductId == OrderedProductID)
                    .OrderBy(x => x.CustomProductCreatedOn)
                        .Select(x => x.CustomerId).FirstOrDefault();
                return DesignerID;
            }

            public void UpdateCustomerPoint(Nullable<Guid> CustomerID, Nullable<int> reward_pts)
            {
                var result = context.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
                result.CustomerPoint += reward_pts;
                context.SaveChanges();

                //SqlConnection con = new SqlConnection("data source=salal.arvixe.com;initial catalog=DIYCommerceV2;persist security info=True;user id=user01;password=qwer1234;multipleactiveresultsets=True;application name=EntityFramework&quot;");
                //string sql = "UPDATE Customers SET CustomerPoint = @RewardPts WHERE CustomerID = @DesignerID";
                //SqlCommand cmd = new SqlCommand(sql, con);
                //con.Open();
                //cmd.Parameters.AddWithValue("@RewardPts", reward_pts);
                //cmd.Parameters.AddWithValue("@DesignerID", CustomerID);
                //cmd.ExecuteNonQuery();  
            }
        #endregion

        #region Customer Address

            /// <summary>
            /// Create Shipping/Secondary Address
            /// </summary>
            /// <param name="address"></param>
            public void CreateShippingAddress(Address address) {
                address.AddressId = Guid.NewGuid();
                address.Type = AddressType.SHIPPING_ADDRESS;
                address.Status = Status.CUSTOMER_ADDRESS_ACTIVE;
                address.AddressCreatedBy = address.CustomerId.ToString();
                address.AddressCreatedOn = DateTime.Now;
                try
                {
                    context.Addresses.Add(address);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }

           /// <summary>
           /// Get address by address id
           /// </summary>
           /// <param name="AddressID"></param>
           /// <returns></returns>
           public Address GetAddress(Guid AddressID)
            {
                return context.Addresses
                    .Where(x => x.AddressId.Equals(AddressID)
                                && x.Status==Status.CUSTOMER_ADDRESS_ACTIVE)
                    .FirstOrDefault();

            }
            
            /// <summary>
            /// Get list of addresses of a customer
            /// </summary>
            /// <param name="AddressID"></param>
            /// <returns></returns>
            public List<Address> GetAddresses(Guid CustomerID){
                return context.Addresses.Where(x => x.CustomerId.Equals(CustomerID)
                                                    && x.Status==Status.CUSTOMER_ADDRESS_ACTIVE)
                                        .ToList();
            }
            
            /// <summary>
            /// Update address record
            /// </summary>
            /// <param name="NewRecord"></param>
            public void UpdateAddress(Address NewRecord) {
                Address address = context.Addresses
                                            .Where(x => x.AddressId.Equals(NewRecord.AddressId) 
                                                        && x.CustomerId.Equals(NewRecord.CustomerId)
                                                        && x.Status==Status.CUSTOMER_ADDRESS_ACTIVE)
                                            .FirstOrDefault();
                address.AddressId = NewRecord.AddressId;
                address.Name = NewRecord.Name;
                address.Address1 = NewRecord.Address1;
                address.State = NewRecord.State;
                address.City = NewRecord.City;
                address.Postcode = NewRecord.Postcode;
                address.Country = NewRecord.Country;
                address.Contact = NewRecord.Contact;
                address.AddressUpdatedOn = DateTime.Now;
                address.AddressUpdatedBy = NewRecord.CustomerId.ToString();

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }
            
            /// <summary>
            /// Shallow delete address record
            /// </summary>
            /// <param name="AddressID"></param>
            public void DeleteAddress(Guid AddressID) {
                Address address = context.Addresses.Where(x => x.AddressId.Equals(AddressID)).FirstOrDefault();
                address.Status = Status.CUSTOMER_ADDRESS_INACTIVE;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
            }
        #endregion

        #region Order
            public void AddNewOrder(Order order)
            {
                order.OrderID = Guid.NewGuid();
                //order.CartListId = order.CartListId;
                order.OrderStatus = Status.ORDER_PENDING;
                try
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }

            }

            public List<vw_OrderHistory> GetOrderHistoryByCustomerID(Guid CustomerID)
            {
                return context.vw_OrderHistory.Where(x => x.CustomerID == CustomerID).ToList();
            }

            public vw_OrderHistory GetOrderHistoryByOrderID(Guid OrderID)
            {
                return context.vw_OrderHistory.Where(x => x.OrderID == OrderID).FirstOrDefault();
            }
        #endregion

            #region Tailor
            /// <summary>
            /// Get tailor by using customer id
            /// </summary>
            /// <param name="CustomerID"></param>
            /// <returns></returns>
            public Tailor GetTailorByCustomerID(Guid CustomerID)
            {
                return context.Tailors
                    .Where(x => x.CustomerID.Equals(CustomerID))
                    .FirstOrDefault();

            }
            public List<vw_TaskHistory> GetTaskHistoryByTailorID(Guid TailorID)
            {
                return context.vw_TaskHistory.Where(x => x.TailorID == TailorID).ToList();

            }

            /// <summary>
            /// Return In Progress Task Count 
            /// </summary>
            /// <param name="TailorID"></param>
            /// <returns></returns>
            public int TaskCount(Guid TailorID)
            {
                //var CartID = context.Carts.Where(x => x.CustomerID.Equals(CustomerID) && x.CartStatus == Status.CART_PENDING).Select(x => x.CartID).FirstOrDefault();
                return context.vw_TaskHistory.Where(x => x.TailorID == TailorID && x.OrderStatus == Status.ORDER_IN_PROGRESS).Count();
            }

            public IEnumerable<vw_TailorDetails> TailorsInfoList()
            {
                return context.vw_TailorDetails.OrderBy(x => x.Specialization);
            }

            public List<OrderAssignment> GetOrderAssignments(Guid TailorID)
            {
                return context.OrderAssignments.Where(x => x.TailorID == TailorID).ToList();
            }

            public List<vw_ReviewsDetails> GetLikesOrderAssignment(Guid TailorID)
            {
                return context.vw_ReviewsDetails.Where(x => x.TailorID == TailorID && x.ReviewCategory == 1).OrderBy(x => x.CommentOn).ToList();
            }

            public List<vw_ReviewsDetails> GetDislikesOrderAssignment(Guid TailorID)
            {
                return context.vw_ReviewsDetails.Where(x => x.TailorID == TailorID && x.ReviewCategory == 0).OrderBy(x => x.CommentOn).ToList();
            }
        #endregion
    }
}
