using Bookify.Domain.Abstractions;

namespace Bookify.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{
    protected static T AssertDomainEventWasRaised<T>(Entity entity) 
        where T : IDomainEvent
    {
        var domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();
        
        if(domainEvent is null)
        {
            throw new Exception($"Expected domain event of type {typeof(T).Name} to be raised, but it was not.");
        }
        
        return domainEvent;
    }
}