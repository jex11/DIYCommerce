//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECWebApp.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Addresses = new HashSet<Address>();
            this.Carts = new HashSet<Cart>();
            this.EmailConfirms = new HashSet<EmailConfirm>();
            this.LastAccesses = new HashSet<LastAccess>();
            this.RoleAssigns = new HashSet<RoleAssign>();
            this.CustomProducts = new HashSet<CustomProduct>();
            this.PasswordResets = new HashSet<PasswordReset>();
            this.Reviews = new HashSet<Review>();
            this.Tailors = new HashSet<Tailor>();
        }
    
        public System.Guid CustomerID { get; set; }
        public Nullable<int> CustomerCode { get; set; }
        public byte[] CustomerImg { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerNRIC { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostcode { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerContact { get; set; }
        public Nullable<int> CustomerPoint { get; set; }
        public System.Guid CustomerCategoryID { get; set; }
        public Nullable<int> CustomerStatus { get; set; }
        public Nullable<System.DateTime> CustomerCreatedOn { get; set; }
        public string CustomerCreatedBy { get; set; }
        public Nullable<System.DateTime> CustomerUpdatedOn { get; set; }
        public string CustomerUpdatedBy { get; set; }
        public string CustomerImgType { get; set; }
    
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<EmailConfirm> EmailConfirms { get; set; }
        public virtual ICollection<LastAccess> LastAccesses { get; set; }
        public virtual ICollection<RoleAssign> RoleAssigns { get; set; }
        public virtual ICollection<CustomProduct> CustomProducts { get; set; }
        public virtual ICollection<PasswordReset> PasswordResets { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Tailor> Tailors { get; set; }
    }
}
