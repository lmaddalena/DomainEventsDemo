using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Repository
{
    interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
    }
}