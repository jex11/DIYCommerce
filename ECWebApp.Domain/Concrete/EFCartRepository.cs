using ECWebApp.Domain.Abstract;
using ECWebApp.Domain.Constant;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Concrete
{
    public class EFCartRepository : ICartRepository
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();

        /// <summary>
        /// Cart Collection per session
        /// </summary>
        private List<CartItem> _Cart = new List<CartItem>();


        public List<CartItem> Cart
        {
            get { return _Cart; }
        }

        public List<CartItem> PendingCartItems
        {
            get { return context.CartItems.Where(x => x.Cart.CartStatus == Status.CART_PENDING).ToList(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Guid GetActiveCart(Guid CustomerID)
        {
            Guid output = context.Carts
                .Where(x => x.CustomerID.Equals(CustomerID) && x.CartStatus == Status.CART_PENDING)
                .Select(x => x.CartID)
                .FirstOrDefault();

            if (output == Guid.Empty)
            {
                // Refresh Cart List
                if (_Cart != null)
                {
                    _Cart = new List<CartItem>();
                }

                Cart newCart = new Cart();
                newCart.CartID = Guid.NewGuid();
                newCart.CartStatus = Status.CART_PENDING;
                newCart.CustomerID = CustomerID;
                newCart.PaymentStatus = Status.PAYMENT_PENDING;
                newCart.CartCreatedOn = DateTime.Now.ToLocalTime();
                newCart.CartCreatedBy = CustomerID.ToString();
                newCart.CartUpdatedOn = DateTime.Now.ToLocalTime();
                newCart.CartUpdatedBy = CustomerID.ToString();

                output = newCart.CartID;

                try
                {
                    context.Carts.Add(newCart);
                    context.SaveChanges();
                    return output;
                    //return _Cart;
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
                }
                //Guid CartId = context.Carts.Where(y => y.CustomerID.Equals(CustomerID) && y.CartStatus == Status.CART_PENDING).Select(y => y.CartID).FirstOrDefault();
                //if (CartId == Guid.Empty)
                //{
                    

                //}
                //_Cart = context.CartItems.Where(x => x.CartId.Equals(CartId)).ToList();

                //return _Cart;
            }

            return output;
        }

        /// <summary>
        /// Initialize Cart List for member
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<CartItem> GetCartList(Guid CustomerId)
        {
            // Refresh Cart List
            if (_Cart != null)
            {
                _Cart = new List<CartItem>();
            }

            Guid CartId = context.Carts.Where(y => y.CustomerID.Equals(CustomerId) && y.CartStatus == Status.CART_PENDING).Select(y => y.CartID).FirstOrDefault();
            if (CartId == Guid.Empty)
            {
                Cart newCart = new Cart();
                newCart.CartID = Guid.NewGuid();
                newCart.CartStatus = Status.CART_PENDING;
                newCart.CustomerID = CustomerId;
                newCart.PaymentStatus = Status.PAYMENT_PENDING;
                newCart.CartCreatedOn = DateTime.Now.ToLocalTime();
                newCart.CartCreatedBy = CustomerId.ToString();
                newCart.CartUpdatedOn = DateTime.Now.ToLocalTime();
                newCart.CartUpdatedBy = CustomerId.ToString();

                CartId = newCart.CartID;

                try 
                {
                    context.Carts.Add(newCart);
                    context.SaveChangesAsync();
                    return _Cart;
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
                }
                
            }
            _Cart = context.CartItems.Where(x => x.CartId.Equals(CartId)).ToList();

            return _Cart;
        }

        /// <summary>
        /// Add Item to Cart
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(CartItem item)
        {
           if(context.CartItems.Any(x => x.CartId.Equals(item.CartId) && x.ProductId.Equals(item.ProductId)))
           {
               CartItem ExistingRecord = context.CartItems
                                                .Where(x => x.CartId.Equals(item.CartId) && x.ProductId.Equals(item.ProductId))
                                                .FirstOrDefault();
               ExistingRecord.CartListTotalQuantity += item.CartListTotalQuantity;
               ExistingRecord.CartListUpdatedOn = DateTime.Now;
           }
           else
           {
               context.CartItems.Add(item); 
           }

           try
           {
               //context.SaveChanges(); 
               context.SaveChangesAsync();  //can try put async
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
           }
           
        }

        /// <summary>
        /// Add multiple Items to Cart
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(IEnumerable<CartItem> items)
        {
            context.CartItems.AddRange(items);
            context.SaveChangesAsync();
        }


        /// <summary>
        /// Update Items in Cart
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(CartItem item)
        {
            var result = context.CartItems.Where(x => x.CartListId.Equals(item.CartListId)).FirstOrDefault();
            //context.CartLists.Remove(result);
            //context.CartLists.Add(item);
            
            result.CartListTotalQuantity = item.CartListTotalQuantity;
            result.CartListUpdatedOn = DateTime.Now;
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove Item from Cart
        /// </summary>
        /// <param name="index"></param>
        public void DeleteItem(Guid CartListID)
        {
            var result = context.CartItems.Where(x => x.CartListId.Equals(CartListID)).FirstOrDefault();
            context.CartItems.Remove(result);
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove multiple items from Cart
        /// </summary>
        /// <param name="index"></param>
        public void DeleteItems(List<Guid> CartListIDs)
        {
            IEnumerable<CartItem> result = Cart.Where(x => CartListIDs.Contains(x.CartListId));
            context.CartItems.RemoveRange(result);
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Return Active Cart Count 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public int CartCount(Guid CustomerID)
        {
            var CartID = context.Carts.Where(x => x.CustomerID.Equals(CustomerID) && x.CartStatus == Status.CART_PENDING).Select(x => x.CartID).FirstOrDefault();
            return context.CartItems.Where(x => x.CartId.Equals(CartID)).Count();
        }

        /// <summary>
        /// Check Out Cart
        /// </summary>
        /// <param name="CartID"></param>
        public void CheckOutCart(string CartID)
        {
            Guid CheckOutCartID = new Guid(CartID);
            var result = context.Carts.Where(x => x.CartID.Equals(CheckOutCartID)).First();
            result.CartStatus = 2;
            result.CartUpdatedOn = DateTime.Now;
            context.SaveChangesAsync();
            //if (context.CartItems.Any(x => x.CartId.Equals(item.CartId) && x.ProductId.Equals(item.ProductId)))
            //{
            //    CartItem ExistingRecord = context.CartItems
            //                                     .Where(x => x.CartId.Equals(item.CartId) && x.ProductId.Equals(item.ProductId))
            //                                     .FirstOrDefault();
            //    ExistingRecord.CartListTotalQuantity += item.CartListTotalQuantity;
            //    ExistingRecord.CartListUpdatedOn = DateTime.Now;
            //}
            //else
            //{
            //    context.CartItems.Add(item);
            //}

            //try
            //{
            //    context.SaveChangesAsync();
            //}

            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// Check Out Cart
        /// </summary>
        /// <param name="CartID"></param>
        //public void CheckOutCart(string CartID)
        //{
        //    Guid CheckOutCartID = new Guid(CartID);
        //    var result = context.Carts.Where(x => x.CartID.Equals(CheckOutCartID)).First();
        //    result.CartStatus = 2;
        //    result.CartUpdatedOn = DateTime.Now;
        //    context.SaveChangesAsync();

        //}
    }
    
}
