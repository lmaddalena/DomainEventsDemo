
namespace DomainEventsDemo.Commands
{
    abstract class Command
    {
        public string CommandName { get; private set; }

        protected Command()
        {
            this.CommandName = this.GetType().Name;
        }

        public abstract bool Execute();
    }
}