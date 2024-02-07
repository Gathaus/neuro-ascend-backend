using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Application.Managers.Concrete;
using Neuro.Domain.Entities;
using Neuro.Domain.Logging;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Logging;
using NLog;
using NLog.Fluent;

namespace Neuro.Application.BackgroundJobs;

public class TestJob : IRecurringJob
{
    public TestJob(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
    }

    [Queue("critical")]
    [RecurringJob("TestJob.cs", "5 * * * * *")]
    public async Task Execute()
    {
    }
}