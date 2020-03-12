using System;
using MediatR;

namespace DomainEventsDemo.DomainEvents
{
    class ProductPriceUpdatedDomainEvent : INotification
    {
        public int ProductId { get; private set; }
        public double OldPrice { get; private set; }
        public double NewPrice { get; private set; }

        public ProductPriceUpdatedDomainEvent(int productId, double oldPrice, double newPrice)
        {
            this.ProductId = productId;
            this.OldPrice = oldPrice;
            this.NewPrice = newPrice;
        }

    }
}