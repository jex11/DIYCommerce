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
    
    public partial class PasswordReset
    {
        public System.Guid PasswordResetId { get; set; }
        public System.Guid CustomerId { get; set; }
        public string Token { get; set; }
        public Nullable<System.DateTime> PasswordResetExpiresOn { get; set; }
        public Nullable<int> PasswordResetStatus { get; set; }
        public Nullable<System.DateTime> PasswordResetCreatedOn { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
