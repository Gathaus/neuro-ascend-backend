using Hangfire.Common;
using Hangfire.States;
using Serilog;

namespace Neuro.Infrastructure.Hangfire.Filters;

public class HangfireExceptionFilter : JobFilterAttribute, IElectStateFilter
{
    public void OnStateElection(ElectStateContext context)
    {
        var failedState = context.CandidateState as FailedState;
        if (failedState != null)
        {
            Log.Error($"Hangfire job failed: {failedState.Exception.Message}");
        }
    }
}