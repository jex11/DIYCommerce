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
    
    public partial class vw_TrialTailor
    {
        public System.Guid TailorID { get; set; }
        public string Specialization { get; set; }
        public Nullable<decimal> AverageRating { get; set; }
        public Nullable<int> AverageElapsedDay { get; set; }
        public Nullable<int> OrderInHand { get; set; }
        public Nullable<int> OrderDone { get; set; }
        public System.Guid AssignID { get; set; }
        public System.Guid OrderID { get; set; }
        public System.Guid TemplateID { get; set; }
    }
}
