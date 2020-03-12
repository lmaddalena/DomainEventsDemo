using System.Threading.Tasks;
using DomainEventsDemo.DomainEvents;
using MediatR;

namespace DomainEventsDemo
{
    interface IMediatorService
    {
        void Dispatch(INotification domainEvent);
    }

}