using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainEventsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DomainEventsDemo.Repository
{
    class CartRepository : ICartRepository
    {
        private CartDataContext _dataContext;

        public CartRepository(CartDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Cart> AddAsync(User user)
        {
            Cart cart = new Cart(user);
            await _dataContext.Carts.AddAsync(cart);
            
            return cart;
        }

        public void DelCart(Cart cart)
        {
            _dataContext.Carts.Remove(cart);
        }

        public async Task<Cart> GetByIdAsync(int cartId)
        {
            var cart = from c in _dataContext.Carts
                       where c.CartId == cartId
                       select c;

            return await cart.SingleOrDefaultAsync();
        }

        public async Task<List<Cart>> GetCartsContainingProduct(int productId)
        {
            var carts = from c in _dataContext.Carts
                        from i in c.CartItems
                        where i.ProductId == productId
                        select c;

            return await carts.ToListAsync();
        }
    }
}