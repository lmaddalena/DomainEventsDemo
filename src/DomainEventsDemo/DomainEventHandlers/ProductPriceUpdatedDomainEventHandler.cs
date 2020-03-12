using System.Threading;
using System.Threading.Tasks;
using DomainEventsDemo.DomainEvents;
using DomainEventsDemo.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DomainEventsDemo.DomainEventHandlers
{    
    class ProductPriceUpdatedDomainEventHandler : INotificationHandler<ProductPriceUpdatedDomainEvent>
    {
        private readonly ILogger<ProductPriceUpdatedDomainEventHandler> _logger;
        private ICartRepository _cartRepository;

        public ProductPriceUpdatedDomainEventHandler(
                ICartRepository cartRepository,
                ILogger<ProductPriceUpdatedDomainEventHandler> logger
            )
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public Task Handle(ProductPriceUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var carts = (_cartRepository.GetCartsContainingProduct(notification.ProductId)).Result;

            // update carts
            foreach (var c in carts)
            {               
                _logger.LogInformation(string.Format("Price of product with id '{0}' changed from {1:N} to {2:N2} in cart with id {3} ", 
                        notification.ProductId, notification.OldPrice, notification.NewPrice, c.CartId));            

                c.UpdateItemPrice(notification.ProductId, notification.NewPrice);

            }

            return Task.CompletedTask;
        }
    }
}