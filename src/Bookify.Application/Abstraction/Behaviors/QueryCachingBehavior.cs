using Bookify.Application.Abstraction.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstraction.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var cacheKey = request.CacheKey;
        
        var cachedResponse = await cacheService.GetAsync<TResponse>(cacheKey, cancellationToken);
        
        if (cachedResponse is not null)
        {
            logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedResponse;
        }

        logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        var result = await next();
        
        if(result.IsSuccess)
            await cacheService.SetAsync(cacheKey, result, request.Expiration, cancellationToken);

        return result;
    }
}
