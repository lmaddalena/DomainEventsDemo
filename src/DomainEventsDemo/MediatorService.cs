using System;
using System.Linq;
using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;

namespace DomainEventsDemo
{    

    class MediatorService : IMediatorService
    {     
        private IMediator _mediator;
        public MediatorService(IMediator mediator)
        {            
            _mediator = mediator;
        }

        public void Dispatch(INotification domainEvent)
        {
            _mediator.Publish(domainEvent);
        }   

    }


}