using Neuro.BackgroundJobs;
using Neuro.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddInfrastructure();
    })
    .Build();



host.Run();