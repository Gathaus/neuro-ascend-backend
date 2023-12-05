using Hangfire;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Serilog;

namespace Neuro.BackgroundJobs.Jobs;

public class SampleJob2 : IRecurringJob
{
    [Queue("default")]
    [RecurringJob("sample-job2", "*/1 * * * *")]
    public void Execute()
    {
        Log.Information("HANGFIRE2 JOB EXECUTED");
        Log.Information("HANGFIRE2 JOB EXECUTED");
        Log.Information("HANGFIRE2 JOB EXECUTED");
        Log.Information("HANGFIRE2 JOB EXECUTED");
        
    }
}
