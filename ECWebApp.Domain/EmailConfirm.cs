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
    
    public partial class EmailConfirm
    {
        public System.Guid EmailConfirmId { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string CustomerEmail { get; set; }
        public string ConfirmationCode { get; set; }
        public Nullable<int> ConfirmationStatus { get; set; }
        public Nullable<System.DateTime> EmailConfirmCreatedOn { get; set; }
        public Nullable<System.DateTime> EmailConfirmEndOn { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}