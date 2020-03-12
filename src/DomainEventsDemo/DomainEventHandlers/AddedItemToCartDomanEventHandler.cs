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
    class AddedItemToCartDomanEventHandler : INotificationHandler<AddedItemToCartDomanEvent>
    {
        private ILogger<AddedItemToCartDomanEventHandler> _logger;
        private IProductRepository _productRepository;

        public AddedItemToCartDomanEventHandler(
                IProductRepository productRepository,
                ILogger<AddedItemToCartDomanEventHandler> logger
            )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public Task Handle(AddedItemToCartDomanEvent notification, CancellationToken cancellationToken)
        {
            Product prod = _productRepository.GetByIdAsync(notification.ProductId).Result;

            if(prod == null)
                throw new NullReferenceException("Product not found");

            _logger.LogInformation(string.Format("N. {0} '{1}' added to cart with id {2} ", 
                    notification.Quantity, prod.Description, notification.CartId));            

            return Task.CompletedTask;
        }
    }
}