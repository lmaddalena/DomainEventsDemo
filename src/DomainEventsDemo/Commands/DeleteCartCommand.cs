using DomainEventsDemo.Models;
using DomainEventsDemo.Repository;

namespace DomainEventsDemo.Commands
{
    class DeleteCartCommand : Command
    {
        Cart _cart;
        ICartRepository _cartRepository;

        public DeleteCartCommand(
            Cart cart,
            ICartRepository cartRepository)
        {
            _cart = cart;
            _cartRepository = cartRepository;
        }
        public override bool Execute()
        {
            System.Console.WriteLine("Delete cart with cartId {0}", _cart.CartId);
            
            _cartRepository.DelCart(_cart);
            
            return true;
        }
    }
}