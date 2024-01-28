using System.Reflection;

namespace Neuro.Application.BackgroundJobs;

public static class JobAssemblyProvider
{
    public static Assembly GetJobAssembly() => typeof(JobAssemblyProvider).Assembly;
}