namespace Neuro.Application.Base.BackgroundJobs;

public interface IRecurringJob
{
    Task Execute();
}
