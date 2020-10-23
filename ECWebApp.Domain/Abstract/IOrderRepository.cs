using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Abstract
{
    public interface IOrderRepository
    {

        vw_CartItemsCategory GetSucceedCartItems(Guid cartItemID);
        List<Specialize> GetSpecializedTailor(Guid templateID);    
        void AssignToTailor(Guid tailorID, Guid orderID);
        List<OrderAssignment> GetTailorOrderAssignment(Guid TailorID);
        void CompleteOrder(Nullable<Guid> OrderID, Nullable<Guid> TailorID, Nullable<int> OrderQuantity);
        void RatingOrder(Nullable<Guid> OrderID, int Rate);

        List<vw_ExpTailorAssignment> GetExperiencedTailor(Guid TemplateID, int condition);
    }
}
