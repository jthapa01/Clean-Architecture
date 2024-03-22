using Microsoft.Extensions.Options;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

internal sealed class ProcessOutboxMessagesJobSetup(IOptions<OutboxOptions> outboxOptions)
    : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessOutboxMessagesJob);

        options
            .AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(jobName))
            .AddTrigger(triggerBuilder =>
                triggerBuilder
                    .ForJob(jobName)
                    .WithSimpleSchedule(scheduleBuilder =>
                        scheduleBuilder.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}