
using System;
using System.Threading.Tasks;
using DomainEventsDemo.Models;

namespace DomainEventsDemo.Commands
{
    class UpdateProductPriceCommand : Command
    {
        private Product _product;
        private double _newPrice;

        public UpdateProductPriceCommand(Product product, double newPrice)
        {
            _product = product;
            _newPrice = newPrice;
        }

        public override bool Execute()
        {
            System.Console.WriteLine("Update price {0}. New Price: {1:N2}", 
                    _product.Description,
                    _newPrice);

            _product.UpdatePrice(_newPrice);

            return true;
        }
    }
}