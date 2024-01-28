using Hangfire;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Domain.Logging;
using NLog.Fluent;

namespace Neuro.Application.BackgroundJobs;

public class TestJob : IRecurringJob
{
    private readonly INeuroLogger neuroLogger;
    
    public TestJob(INeuroLogger neuroLogger)
    {
        this.neuroLogger = neuroLogger;
    }
    
    [Queue("critical")]
    [RecurringJob("TestJob", "0 */1 * * * *")]
    public async Task Execute()
    {
        await neuroLogger.LogError(new Exception("TestJob"), "TestJob");
        
    }
}