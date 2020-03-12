using System.Threading.Tasks;
using DomainEventsDemo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainEventsDemo.Repository 
{
    class ProductRepository : IProductRepository
    {
        private CartDataContext _dataContext;

        public ProductRepository(CartDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Product> AddAsync(string description, double price)
        {
            Product p = new Product(0, description, price);
            await _dataContext.Products.AddAsync(p);
            return p;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var prods = from p in _dataContext.Products
                        select p;

            return await prods.ToListAsync();

        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            var prod = from p in _dataContext.Products
                      where p.ProductId == productId
                      select p;

            return await prod.SingleOrDefaultAsync();
        }

        public void Remove(Product product)
        {
            _dataContext.Remove<Product>(product);
        }
    }
}