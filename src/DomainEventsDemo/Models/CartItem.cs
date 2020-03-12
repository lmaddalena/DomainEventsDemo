using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DomainEventsDemo.Models
{
    class CartItem : Entity
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        
    }
}