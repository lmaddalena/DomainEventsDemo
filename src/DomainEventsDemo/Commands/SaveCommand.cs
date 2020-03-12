using DomainEventsDemo.Models;
using DomainEventsDemo.Repository;

namespace DomainEventsDemo.Commands
{
    class SaveCommand : Command
    {
        IUnitOfWork _unitOfWork;
        public SaveCommand(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public override bool Execute()
        {
            System.Console.WriteLine("\nSaving....\n");
            _unitOfWork.SaveAsync().Wait();
                        
            return true;
        }
    }
}