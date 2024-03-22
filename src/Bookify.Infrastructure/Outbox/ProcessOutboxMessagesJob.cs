using System.Data;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IJob
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages");
        
        using var connection = sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        
        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            var exception = default(Exception);

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;
                
                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch(Exception caughtException)
            {
                logger.LogError(
                    caughtException, 
                    "Exception while processing outbox message {MessageId}", 
                    outboxMessage.Id);
                
                exception = caughtException;
            }
            
            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();
        
        logger.LogInformation("Finished processing outbox messages");
    }
    
    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection, 
        IDbTransaction transaction)
    {
        const string sql = """
                           SELECT
                               id AS Id,
                               content AS Content
                           FROM outbox_messages
                           WHERE processed_on_utc IS NULL
                           ORDER BY occurred_on_utc
                           LIMIT @BatchSize
                           FOR UPDATE
                           """;

        return (await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            new
            {
                _outboxOptions.BatchSize
            },
            transaction)).ToList();
    }
    
    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = """
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id
            """;
        
        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction);
    }
    
    private sealed record OutboxMessageResponse(Guid Id, string Content);
}