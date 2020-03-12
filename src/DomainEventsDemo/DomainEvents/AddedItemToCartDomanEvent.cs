
using MediatR;

namespace DomainEventsDemo.DomainEvents
{
    class AddedItemToCartDomanEvent : INotification
    {
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public AddedItemToCartDomanEvent(int cartId, int productId, int quantity)
        {
            this.CartId = cartId;
            this.ProductId = productId;
            this.Quantity = quantity;
        }
    }
}
