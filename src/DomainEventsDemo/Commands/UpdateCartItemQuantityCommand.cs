
using System;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Commands
{
    class UpdateCartItemQuantityCommand : Command
    {
        private Cart _cart;
        private Product _product;
        private int _quantity;

        public UpdateCartItemQuantityCommand(Cart cart, Product product, int quantity)
        {
            _cart = cart;
            _product = product;
            _quantity = quantity;
        }

        public override bool Execute()
        {
            System.Console.WriteLine("Update quantity of product '{0}' in {1}'s cart, new quantity: {2}",
                _product.Description, 
                _cart.User.UserId, 
                _quantity);

            _cart.UpdateItemQuantity(_product, _quantity);


            return true;
        }
    }
}