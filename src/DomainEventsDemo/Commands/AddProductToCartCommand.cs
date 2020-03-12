
using System;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Commands
{
    class AddProductToCartCommand : Command
    {
        private Cart _cart;
        private Product _product;
        private int _quantity;

        public AddProductToCartCommand(Cart cart, Product product, int quantity)
        {
            _cart = cart;
            _product = product;
            _quantity = quantity;
        }

        public override bool Execute()
        {
            System.Console.WriteLine("Add n. {0} '{1}' to {2}'s cart", 
                    _quantity, 
                    _product.Description, 
                    _cart.User.UserId);

            _cart.AddItemToCart(_product, _quantity);

            return true;
        }
    }
}