using Hangfire.Dashboard;

namespace Neuro.Infrastructure.Hangfire.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        //TODO - Add authorization logic will be added here
        return true;  
    }
}