using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DomainEventsDemo.Models
{
    class User : Entity
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<Cart> Carts { get; set; }

        private List<Cart> _carts;

        public virtual IReadOnlyCollection<Cart> Carts => _carts?.AsReadOnly();


        public User()
        {
            _carts = new List<Cart>();
        }
        public Cart AddCart()
        {
            Cart cart = new Cart(this);
            _carts.Add(cart);
            return cart;
        }


    }
}
