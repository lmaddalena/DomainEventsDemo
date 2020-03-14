# DomainEventsDemo
## How to implement Domain Events in Domain-Driven Design
The Domain-Driven Design (DDD) is an approach to software development based on the domain entity model and their logic.

A model acts as a universal and rigorous language ([Ubiquitous Language](https://martinfowler.com/bliki/UbiquitousLanguage.html)) to help communication between software developers and domain experts.

When models are large, complexity can be reduced by splitting the model into smaller independent models according the [Bounded Context](https://martinfowler.com/bliki/BoundedContext.html) design pattern (BC).

Each Bounded Context represents an application area independent of the others, expresses its Ubiquitous Language, implements its application logic in a consistent way. 

In an e-commerce application, examples of Bounded Context could be: "Product Context", "Orders Context", "Shipping Context", etc.

Each Bounded Context is made up of entities. Entities represent domain objects and are primarily defined by their identity, continuity, and persistence over time, and not only by the attributes that comprise them.

> Domain entities must implement both data attributes and behaviour (in contrast with "[anemic-domain  model](https://martinfowler.com/bliki/AnemicDomainModel.html)").

An action that modifies data on an entity could have side effects on other entities. For example, let's think about the implementation of a "Shopping cart" in an e-commerce application. If the price of a product is changed, it is necessary to update all the carts containing that product and highlight
the price change so that the user can decide whether to continue with the purchase or not.

Therefore a way is needed to "notify" something that happens on a domain entity to other entities of the same domain (in-process), this is done through Domain Events.

In this demo there is an example of implementation of the Domain Event Pattern suggested by Jimmy Bogard in his blog [A better domain events pattern](https://lostechies.com/jimmybogard/2014/05/13/a-better-domain-events-pattern/).

In the method that raises the domain event, we record a domain event on our entity:

    public void UpdatePrice(double newPrice)
    {
        var @event = new ProductPriceUpdatedDomainEvent(this.ProductId, this.Price, newPrice);
        this.AddDomainEvent(@event);

        this.Price = newPrice;
    }

Just before we commit our transaction, we dispatch our events to their respective handlers:

    public async Task<int> SaveAsync()
    {
        DispatchDomainEvents();
        return await _dataContext.SaveChangesAsync();
    }


### Run the Demo
Running the demo you should see something like this:

    Users:
    ---------------------------------
    npaul:  Paul N.
    jdoe:   John Doe


    Products:
    ---------------------------------
    1: Bag           10,00
    2: Glasses       17,50
    3: Mug            5,99
    4: T-Shirt        8,00

    Create carte for user npaul
    Create carte for user jdoe

    Saving....

    Add n. 5 'Bag' to npaul's cart
    Add n. 3 'Glasses' to npaul's cart
    Update quantity of product 'Bag' in npaul's cart, new quantity: 6

    Saving....

    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 5 'Bag' added to cart with id 99
    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 3 'Glasses' added to cart with id 99
    info: DomainEventsDemo.DomainEventHandlers.UpdatedCartItemQuantityDomainEventHandler[0]
        Quantity of product 'Bag' changed from 5 to 6 in cart with id 99

    npaul's cart
    ----------------------------------
    n. 6    Bag        10,00
    n. 3    Glasses    17,50

    Total amount: 112,5

    Add n. 1 'Bag' to jdoe's cart
    Add n. 3 'Glasses' to jdoe's cart
    Add n. 2 'Mug' to jdoe's cart
    Add n. 2 'T-Shirt' to jdoe's cart

    Saving....

    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 1 'Bag' added to cart with id 100
    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 3 'Glasses' added to cart with id 100
    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 2 'Mug' added to cart with id 100
    info: DomainEventsDemo.DomainEventHandlers.AddedItemToCartDomanEventHandler[0]
        N. 2 'T-Shirt' added to cart with id 100

    jdoe's cart
    ----------------------------------
    n. 1    Bag        10,00
    n. 3    Glasses    17,50
    n. 2    Mug         5,99
    n. 2    T-Shirt     8,00

    Total amount: 90,48

    Update price Glasses. New Price: 15,00

    Saving....

    info: DomainEventsDemo.DomainEventHandlers.ProductPriceUpdatedDomainEventHandler[0]
        Price of product with id '2' changed from 17,50 to 15,00 in cart with id 99
    info: DomainEventsDemo.DomainEventHandlers.ProductPriceUpdatedDomainEventHandler[0]
        Price of product with id '2' changed from 17,50 to 15,00 in cart with id 100

    npaul's cart
    ----------------------------------
    n. 6    Bag        10,00
    n. 3    Glasses    15,00

    Total amount: 105


    jdoe's cart
    ----------------------------------
    n. 1    Bag        10,00
    n. 3    Glasses    15,00
    n. 2    Mug         5,99
    n. 2    T-Shirt     8,00

    Total amount: 82,98

    Delete cart with cartId 99
    Delete cart with cartId 100

    Saving....

    Update price Glasses. New Price: 17,50

    Saving....


