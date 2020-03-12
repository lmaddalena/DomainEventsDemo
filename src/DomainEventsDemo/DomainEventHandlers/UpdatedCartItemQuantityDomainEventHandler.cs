using System;
using System.Threading;
using System.Threading.Tasks;
using DomainEventsDemo.DomainEvents;
using DomainEventsDemo.Models;
using DomainEventsDemo.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DomainEventsDemo.DomainEventHandlers
{    
    class UpdatedCartItemQuantityDomainEventHandler : INotificationHandler<UpdatedCartItemQuantityDomainEvent>
    {
        private readonly ILogger<UpdatedCartItemQuantityDomainEventHandler> _logger;
        private IProductRepository _productRepository;

        public UpdatedCartItemQuantityDomainEventHandler(
                IProductRepository productRepository,
                ILogger<UpdatedCartItemQuantityDomainEventHandler> logger
            )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public Task Handle(UpdatedCartItemQuantityDomainEvent notification, CancellationToken cancellationToken)
        {
            Product prod = _productRepository.GetByIdAsync(notification.ProductId).Result;

            if(prod == null)
                throw new NullReferenceException("Product not found");

            _logger.LogInformation(string.Format("Quantity of product '{0}' changed from {1} to {2} in cart with id {3} ", 
                    prod.Description, notification.OldQuantity, notification.NewQuantity, notification.CartId));            

            return Task.CompletedTask;
        }
    }
}