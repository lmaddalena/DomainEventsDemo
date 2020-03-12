using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEventsDemo.Repository
{
    interface IUnitOfWork
    {
        void Save();
        Task<int> SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        void RollbackTransaction();
        IProductRepository GetProductRepository();
        IUserRepository GetUserRepository();
        ICartRepository GetCartRepository();
        
    }
}