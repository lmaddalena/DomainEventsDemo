using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Repository
{
    interface IProductRepository
    {
        Task<Product> AddAsync(string description, double price);
        void Remove(Product item);
        Task<Product> GetByIdAsync(int productId);
        Task<List<Product>> GetAllAsync();
    }

}