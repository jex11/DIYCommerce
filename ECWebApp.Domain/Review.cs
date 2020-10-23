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
    
    public partial class Review
    {
        public System.Guid ProductReviewId { get; set; }
        public System.Guid CustomerID { get; set; }
        public System.Guid ProductID { get; set; }
        public string ProductReview { get; set; }
        public int ProductReviewRate { get; set; }
        public System.DateTime ProductReviewCreatedOn { get; set; }
        public string ProductReviewCreatedBy { get; set; }
        public Nullable<System.DateTime> ProductReviewUpdatedOn { get; set; }
        public string ProductReviewUpdatedBy { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}