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
    
    public partial class vw_CartItemTemplateID
    {
        public System.Guid OrderID { get; set; }
        public System.Guid CartId { get; set; }
        public System.Guid CartListId { get; set; }
        public System.Guid ProductId { get; set; }
        public System.Guid TemplateID { get; set; }
        public int CartStatus { get; set; }
        public int CartListTotalQuantity { get; set; }
        public Nullable<System.DateTime> OrderBeginTime { get; set; }
        public Nullable<System.DateTime> OrderFinishedTime { get; set; }
    }
}
