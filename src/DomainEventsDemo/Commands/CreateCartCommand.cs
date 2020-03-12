using DomainEventsDemo.Models;

namespace DomainEventsDemo.Commands
{
    class CreateCartCommand : Command
    {
        User _user;
        public Cart Cart { get; private set; }  
        public CreateCartCommand(User user)
        {
            _user = user;
        }
        public override bool Execute()
        {
            System.Console.WriteLine("Create carte for user {0}", _user.UserId);
            
            this.Cart = _user.AddCart();
            return true;
        }
    }
}