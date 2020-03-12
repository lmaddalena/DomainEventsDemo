using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Repository
{
    interface ICartRepository
    {
        Task<Cart> AddAsync(User user);
        Task<Cart> GetByIdAsync(int cartId);
        void DelCart(Cart cart);
        Task<List<Cart>> GetCartsContainingProduct(int productId);
    }
}