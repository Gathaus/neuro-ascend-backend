using Hangfire;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.BackgroundJobs.Attributes;
using Serilog;

namespace Neuro.BackgroundJobs.Jobs;

public class SampleJob : IRecurringJob
{
    [Queue("critical")]
    [RecurringJob("sample-job", "*/1 * * * *")]
    public void Execute()
    {
        Log.Information("HANGFIRE1 JOB EXECUTED");
        Log.Information("HANGFIRE1 JOB EXECUTED");
        Log.Information("HANGFIRE1 JOB EXECUTED");
        Log.Information("HANGFIRE1 JOB EXECUTED");   
    }
}
