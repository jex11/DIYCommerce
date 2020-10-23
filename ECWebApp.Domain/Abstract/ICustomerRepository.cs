using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Abstract
{
    public interface ICustomerRepository
    {
        
        
        //Customer Authentication
        bool SignUp(Customer customer); // this is the interface
        bool ValidateEmail(String email);
        Task<string> GenerateEmailConfirmationTokenAsync(Customer Result);
        bool Authenticate(Customer Result, bool RememberMe);
        bool VerifyAccount(Guid CustomerID, String token);

        //Customer Details
        IEnumerable<Customer> Customers{ get; }
        IEnumerable<Customer> DesignCustomers { get; }
        Customer GetCustomerByID(Guid CustomerID);
        Tailor GetTailorByCustomerID(Guid CustomerID);
        Guid GetCustomerID(Customer customer);
        void UpdateCustomerUsername(Customer NewRecord);
        void UpdateCustomerContactNo(Customer NewRecord); 
        void UpdateCustomerEmail(Customer NewRecord); 
        void UpdateCustomerPassword(Customer NewRecord);
        void UpdateCustomerProfileImage(Customer NewRecord);
        Guid GetDesignerID(Guid OrderedProductID);
        void UpdateCustomerPoint(Nullable<Guid> CustomerID, Nullable<int> reward_pts);
        
        //Customer Address
        void CreateShippingAddress(Address address);
        Address GetAddress(Guid AddressID);
        List<Address> GetAddresses(Guid CustomerID);
        void UpdateAddress(Address NewRecord);
        void DeleteAddress(Guid AddressID);

        //Order
        void AddNewOrder(Order order);

        List<vw_OrderHistory> GetOrderHistoryByCustomerID(Guid CustomerID);

        vw_OrderHistory GetOrderHistoryByOrderID(Guid OrderID);

        //Task History
        List<vw_TaskHistory> GetTaskHistoryByTailorID(Guid TailorID);

        IEnumerable<vw_TailorDetails> TailorsInfoList();

        List<OrderAssignment> GetOrderAssignments(Guid TailorID);

        List<vw_ReviewsDetails> GetLikesOrderAssignment(Guid TailorID);

        List<vw_ReviewsDetails> GetDislikesOrderAssignment(Guid TailorID);

        int TaskCount(Guid TailorID);
    }
}
