using DomainEventsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DomainEventsDemo.Repository
{
    class CartDataContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }

        private string _connectionString = null;        
        
        public CartDataContext(): base()
        {
            
        }
        public CartDataContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public CartDataContext(DbContextOptions<CartDataContext> options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(string.IsNullOrEmpty(this._connectionString))
                optionsBuilder.UseSqlite("Data Source=shoppingcart.db");
            else
                optionsBuilder.UseSqlite(this._connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add keys to CartItem entity
            modelBuilder.Entity<CartItem>()
                .HasKey(c => new { c.CartId, c.ProductId });
            
            // seed data
            modelBuilder.Entity<Product>().HasData(
                new Product[] {
                    new Product(1, "Bag", 10.0),
                    new Product(2, "Glasses", 20.50 ),
                    new Product(3, "Mug", 5.99 ),
                    new Product(4, "T-Shirt", 8.00 )
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User[] {
                    new User() { UserId = "npaul", Name = "Paul N."},
                    new User() { UserId = "jdoe", Name = "John Doe" }
                }
            );
        }     
    }
}