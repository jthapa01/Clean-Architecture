using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Abstraction.Caching;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    public string CacheKey { get; }
    
    public TimeSpan? Expiration { get; }
}