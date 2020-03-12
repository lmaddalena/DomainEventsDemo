using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using DomainEventsDemo.DomainEvents;

namespace DomainEventsDemo.Models
{
    class Cart : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; private set; }
        public DateTime CreationDate { get; private set; }

        public double TotalAmount { get; private set; }
        
        //public virtual ICollection<CartItem> CartItems { get; private set; }
        
        private List<CartItem> _cartItems;
        public virtual IReadOnlyCollection<CartItem> CartItems => _cartItems?.AsReadOnly();
        public virtual User User { get; private set; }

        public Cart()
        {
            _cartItems = new List<CartItem>();
        }

        public Cart(User user)        
        {
            this.User = user;
            this.CreationDate = DateTime.Now;
            _cartItems = new List<CartItem>();
        }

        public void AddItemToCart(Product product, int quantity)
        {
            var i = this.CartItems.Where(i => i.ProductId == product.ProductId).FirstOrDefault();
            if(i != null)
            {
                UpdateItemQuantity(product, quantity);
            }
            else
            {
                _cartItems.Add(new CartItem(){
                    CartId = this.CartId,
                    ProductDescription = product.Description,
                    ProductId = product.ProductId,
                    ProductPrice = product.Price,
                    Quantity = quantity
                });

                this.TotalAmount += quantity * product.Price;
                
                var e = new AddedItemToCartDomanEvent(this.CartId, product.ProductId, quantity);
                this.AddDomainEvent(e);
            }

        }

        public void UpdateItemQuantity(Product product, int quantity)
        {
            var i = this.CartItems.Where(i => i.ProductId == product.ProductId).FirstOrDefault();
            if(i != null)
            {
                var e = new UpdatedCartItemQuantityDomainEvent(this.CartId, product.ProductId, i.Quantity, quantity);
                this.AddDomainEvent(e);

                this.TotalAmount -= i.Quantity * i.ProductPrice;                
                i.Quantity = quantity;                
                this.TotalAmount += i.Quantity * i.ProductPrice;
            }
        }

        public void UpdateItemPrice(int productId, double newPrice)
        {
            var i = this.CartItems.Where(i => i.ProductId == productId).FirstOrDefault();
            if(i != null)
            {
                this.TotalAmount -= i.Quantity * i.ProductPrice;                
                i.ProductPrice = newPrice;
                this.TotalAmount += i.Quantity * i.ProductPrice;
            }

        }
    }
}