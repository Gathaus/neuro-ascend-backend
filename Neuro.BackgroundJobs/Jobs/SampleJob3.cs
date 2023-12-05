using Hangfire;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Serilog;

namespace Neuro.BackgroundJobs.Jobs;

public class SampleJob3 : IRecurringJob
{
    [Queue("default")]
    [RecurringJob("sample-job3", "*/1 * * * *")]
    public void Execute()
    {
        Log.Information("HANGFIRE3 JOB EXECUTED");
        Log.Information("HANGFIRE3 JOB EXECUTED");
        Log.Information("HANGFIRE3 JOB EXECUTED");
        Log.Information("HANGFIRE3 JOB EXECUTED");
        
    }
}
