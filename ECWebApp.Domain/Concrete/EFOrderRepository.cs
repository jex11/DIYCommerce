using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();


        /// <summary>
        /// Get the succeed CartItems
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public vw_CartItemsCategory GetSucceedCartItems(Guid cartItemID)
        {
            // Refresh Cart List
            //if (_Cart != null)
            //{
            //    _Cart = new List<CartItem>();
            //}

            return context.vw_CartItemsCategory.Where(x => x.CartListId == cartItemID).FirstOrDefault();
        }

        public List<Specialize> GetSpecializedTailor(Guid templateID)
        {
            return context.Specializes.Where(x => x.TemplateID.Equals(templateID)).ToList();
        }
       
        public void AssignToTailor(Guid tailorID, Guid orderID)
        {
            OrderAssignment assignment = new OrderAssignment();
            assignment.AssignID = Guid.NewGuid();
            assignment.TailorID = tailorID;
            assignment.OrderID = orderID;
            assignment.OrderBeginTime = DateTime.Now;
            context.OrderAssignments.Add(assignment);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
        }

        public List<OrderAssignment> GetTailorOrderAssignment(Guid TailorID)
        {
            return context.OrderAssignments
                .Where(x => x.TailorID.Equals(TailorID))
                .ToList();
        }

        public List<vw_ExpTailorAssignment> GetExperiencedTailor(Guid TemplateID, int condition)
        {
            switch (condition)
            {
                case 1: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating >= 3)
                                .Where(x => x.AverageElapsedDay <= 7)
                                .Where(x => x.OrderInHand < 3)
                                .ToList();
                    break;
                case 2: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating >= 3)
                                .Where(x => x.AverageElapsedDay <= 7)
                                .Where(x => x.OrderInHand >= 3 && x.OrderInHand < 5)
                                .ToList();
                    break;
                case 3: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating >= 3)
                                .Where(x => x.AverageElapsedDay > 7)
                                .Where(x => x.OrderInHand < 3)
                                .ToList();
                    break;
                case 4: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating >= 3)
                                .Where(x => x.AverageElapsedDay > 7)
                                .Where(x => x.OrderInHand >= 3 && x.OrderInHand < 5)
                                .ToList();
                    break;
                case 5: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating < 3)
                                .Where(x => x.AverageElapsedDay <= 7)
                                .Where(x => x.OrderInHand < 3)
                                .ToList();
                    break;
                case 6: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating < 3)
                                .Where(x => x.AverageElapsedDay <= 7)
                                .Where(x => x.OrderInHand >= 3 && x.OrderInHand < 5)
                                .ToList();
                    break;
                case 7: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating < 3)
                                .Where(x => x.AverageElapsedDay > 7)
                                .Where(x => x.OrderInHand < 3)
                                .ToList();
                    break;
                case 8: return context.vw_ExpTailorAssignment.Where(x => x.TemplateID == TemplateID)
                                .Where(x => x.AverageRating < 3)
                                .Where(x => x.AverageElapsedDay > 7)
                                .Where(x => x.OrderInHand >= 3 && x.OrderInHand < 5)
                                .ToList();
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// Complete Order in Order
        /// </summary>
        /// <param name="OrderID"></param>
        public void CompleteOrder(Nullable<Guid> OrderID, Nullable<Guid> TailorID, Nullable<int> OrderQuantity)
        {
            var result = context.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault();
            var result1 = context.OrderAssignments.Where(x => x.OrderID == OrderID).FirstOrDefault();
            var result2 = context.Tailors.Where(x => x.TailorID == TailorID).FirstOrDefault();
            //context.CartLists.Remove(result);
            //context.CartLists.Add(item);

            result.OrderStatus = Status.ORDER_DELIVERED;
            result.OrderUpdatedOn = DateTime.Now;
            result1.OrderEndTime = DateTime.Now;
            result2.OrderInHand -= OrderQuantity;
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Rating for Order
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="Rate"></param>
        public void RatingOrder(Nullable<Guid> OrderID, int Rate)
        {
            var result = context.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault();
            var result1 = context.OrderAssignments.Where(x => x.OrderID == OrderID).FirstOrDefault();
            //context.CartLists.Remove(result);
            //context.CartLists.Add(item);

            result.OrderStatus = Status.ORDER_DONE;
            result.OrderUpdatedOn = DateTime.Now;
            result1.CustomerReview = Rate;
            context.SaveChangesAsync();
        }
    }
}