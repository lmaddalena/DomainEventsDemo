using System.Threading.Tasks;
using DomainEventsDemo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainEventsDemo.Repository
{
    class UserRepository : IUserRepository
    {
        private CartDataContext _dataContext;
        public UserRepository(CartDataContext dataContext)
        {
            _dataContext = dataContext;
        }        
        public async Task<List<User>> GetAllAsync()
        {
            var users = from u in _dataContext.Users
                        select u;

            return await users.ToListAsync();
            
        }
    }
}