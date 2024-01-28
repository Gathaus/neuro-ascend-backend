using Hangfire;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Domain.Logging;
using NLog.Fluent;

namespace Neuro.Application.BackgroundJobs;

public class TestJob1 : IRecurringJob
{
    [Queue("critical")]
    [RecurringJob("TestJob1", "0 */1 * * * *")]
    public async Task Execute()
    {
        
        Console.WriteLine("TestJob1");
        
        
    }
}