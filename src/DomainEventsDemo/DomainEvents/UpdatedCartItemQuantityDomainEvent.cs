using MediatR;

namespace DomainEventsDemo.DomainEvents
{
    class UpdatedCartItemQuantityDomainEvent : INotification
    {
        public int CartId { get; set; }
        public int ProductId { get; private set; } 
        public int OldQuantity { get; private set; }
        public int NewQuantity { get; private set; }

        public UpdatedCartItemQuantityDomainEvent(int cartId, int productId, int oldQuantity, int newQuantity)
        {
            this.CartId = cartId;
            this.ProductId = productId;
            this.OldQuantity = oldQuantity;
            this.NewQuantity = newQuantity;
        }

    }
}