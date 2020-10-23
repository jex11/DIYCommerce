using ECWebApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Abstract
{
    public interface ICartRepository
    {
        List<CartItem> Cart { get; }
        List<CartItem> PendingCartItems { get; }
        List<CartItem> GetCartList(Guid CustomerId);

        int CartCount(Guid CustomerID);
        void AddItem(CartItem item);
        void AddItems(IEnumerable<CartItem> items);
        void UpdateItem(CartItem item);
        void DeleteItem(Guid index);
        void DeleteItems(List<Guid> index);
        Guid GetActiveCart(Guid CustomerID);
        void CheckOutCart(string CartID);

    }
}
