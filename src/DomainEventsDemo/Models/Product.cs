using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using DomainEventsDemo.DomainEvents;

namespace DomainEventsDemo.Models
{
    class Product : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int ProductId { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public Product(int productId, string description, double price)
        {
            this.ProductId = productId;
            this.Description = description;
            this.Price = price;
        }

        public Product():base()
        {
        }

        public void UpdatePrice(double newPrice)
        {
            var @event = new ProductPriceUpdatedDomainEvent(this.ProductId, this.Price, newPrice);
            this.AddDomainEvent(@event);

            this.Price = newPrice;
        }


    }
}