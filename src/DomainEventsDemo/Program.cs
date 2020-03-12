using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DomainEventsDemo.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using DomainEventsDemo.Models;
using DomainEventsDemo.Commands;

namespace DomainEventsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // create and configure the service container
            IServiceCollection serviceCollection = ConfigureServices();

            // build the service provider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IUnitOfWork uow = serviceProvider.GetService<IUnitOfWork>();

            // get all users            
            var users = await uow.GetUserRepository().GetAllAsync();
            PrintUsers(users);

            // get all products
            var prods = await uow.GetProductRepository().GetAllAsync();
            PrintProducts(prods);

            Command cmd;
            Command saveCommand = new SaveCommand(uow);

            // create carts for users[0] & users[1]
            cmd = new CreateCartCommand(users[0]);
            cmd.Execute();

            cmd = new CreateCartCommand(users[1]);
            cmd.Execute();

            var cart1 = users[0].Carts.Last();           
            var cart2 = users[1].Carts.Last();

            saveCommand.Execute();

            // some operation on npaul's cart
            cmd = new AddProductToCartCommand(cart1, prods[0], 5);
            cmd.Execute();

            cmd = new AddProductToCartCommand(cart1, prods[1], 3);
            cmd.Execute();

            cmd = new UpdateCartItemQuantityCommand(cart1, prods[0], 6);
            cmd.Execute();

            saveCommand.Execute();

            PrintCart(cart1);

            // some operation on jdoe's cart
            cmd = new AddProductToCartCommand(cart2, prods[0], 1);
            cmd.Execute();

            cmd = new AddProductToCartCommand(cart2, prods[1], 3);
            cmd.Execute();

            cmd = new AddProductToCartCommand(cart2, prods[2], 2);
            cmd.Execute();

            cmd = new AddProductToCartCommand(cart2, prods[3], 2);
            cmd.Execute();

            saveCommand.Execute();

            PrintCart(cart2);
           
            cmd = new UpdateProductPriceCommand(prods[1], 15);
            cmd.Execute();

            saveCommand.Execute();

            PrintCart(cart1);
            PrintCart(cart2);

            // cleaning
            cmd = new DeleteCartCommand(cart1, uow.GetCartRepository());
            cmd.Execute();

            cmd = new DeleteCartCommand(cart2, uow.GetCartRepository());
            cmd.Execute();

            saveCommand.Execute();

            cmd = new UpdateProductPriceCommand(prods[1], 17.50);
            cmd.Execute();

            saveCommand.Execute();

        }

        private static void PrintCart(Cart cart)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("{0}'s cart", cart.User.UserId);
            System.Console.WriteLine("----------------------------------");
            foreach (var item in cart.CartItems)
            {
                System.Console.WriteLine("n. {0}\t{1,-10}{2,6:N2}",
                    item.Quantity,
                    item.ProductDescription,
                    item.ProductPrice);
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Total amount: {0}\n", cart.TotalAmount);
        }

        private static void PrintProducts(List<Product> prods)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Products:");
            System.Console.WriteLine("---------------------------------");

            prods.ForEach(p => System.Console.WriteLine("{0}: {1,-10}\t{2,6:N2}",
                    p.ProductId, 
                    p.Description, 
                    p.Price
                )
            );

            System.Console.WriteLine();

        }

        private static void PrintUsers(List<User> users)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Users:");
            System.Console.WriteLine("---------------------------------");

            users.ForEach(u => System.Console.WriteLine(u.UserId + ":\t" + u.Name));

            System.Console.WriteLine();
        }

        private static IServiceCollection ConfigureServices()
        {            
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);     

            IConfigurationRoot configuration = builder.Build();

            IServiceCollection service = new ServiceCollection();

            /*
            * Service Lifetimes
            * ====================================================================================
            * There are three service lifetimes in ASP.NET Core Dependency Injection:
            *
            * 1. Transient services are created every time they are injected or requested.
            * 2. Scoped services are created per scope. In a web application, every web request 
            *    creates a new separated service scope. 
            *    That means scoped services are generally created per web request.
            * 3. Singleton services are created per DI container. That generally means that they 
            *    are created only one time per application and then used for whole the application life time.

            * Good Practices
            * ====================================================================================
            * - Register your services as transient wherever possible. Because it’s simple to design 
            *   transient services. You generally don’t care about multi-threading and memory leaks 
            *   and you know the service has a short life.
            *
            * - Use scoped service lifetime carefully since it can be tricky if you create child service 
            *   scopes or use these services from a non-web application.
            *
            * - Use singleton lifetime carefully since then you need to deal with multi-threading and 
            *   potential memory leak problems.
            *
            * - Do not depend on a transient or scoped service from a singleton service. 
            *   Because the transient service becomes a singleton instance when a singleton service 
            *   injects it and that may cause problems if the transient service is not designed to support 
            *   such a scenario. ASP.NET Core’s default DI container already throws exceptions in such cases.
            *
            */

            service.AddLogging(configure => 
                {
                    configure.AddConsole();
                    configure.AddConfiguration(configuration.GetSection("Logging"));
                }).Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
                .AddDbContext<CartDataContext>()
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient<ICartRepository, CartRepository>()
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IMediatorService, MediatorService>();
            
            return service;
        }        
    }
}
