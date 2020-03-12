using System;
using System.Data;
using System.Threading.Tasks;
using DomainEventsDemo.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DomainEventsDemo.Repository
{
    class UnitOfWork : IUnitOfWork
    {
        private CartDataContext _dataContext;
        private ICartRepository _cartRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        private ILogger _logger;
        private IMediatorService _mediatorService;
        private IDbContextTransaction _currentTransaction;
        public UnitOfWork(            
            CartDataContext dataContext,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IMediatorService mediatorService,
            ILogger<UnitOfWork> logger)
        {
            _dataContext = dataContext;            
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mediatorService = mediatorService;
            _logger = logger;
            
        }

        public ICartRepository GetCartRepository()
        {
            return _cartRepository;
        }

        public IProductRepository GetProductRepository()
        {
            return _productRepository;
        }

        public IUserRepository GetUserRepository()
        {
            return _userRepository;
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }


        // save changes
        public void Save()
        {
            DispatchDomainEvents();
            _dataContext.SaveChanges();
        }

        // save changes asynchronly
        public async Task<int> SaveAsync()
        {
            DispatchDomainEvents();

            // EF Core does not support multiple parallel operations being run on the same context instance. 
            // You should always wait for an operation to complete before beginning the next operation. 
            // This is typically done by using the await keyword on each asynchronous operation.

            return await _dataContext.SaveChangesAsync();
        }

        private void DispatchDomainEvents()
        {

            var domainEventEntities = _dataContext.ChangeTracker.Entries<Entity>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents != null && po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventEntities.ToArray())
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();
                foreach (var domainEvent in events)
                {
                    _mediatorService.Dispatch(domainEvent);
                }

            }            
        }
    }
}