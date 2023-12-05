using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neuro.Application.Base.Result;

namespace Neuro.Application.Base.Service;

public class BaseBusinessService
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ILogger<BaseBusinessService> _logger;
    
    public BaseBusinessService(IServiceProvider serviceProvider, ILogger<BaseBusinessService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<Result<TResponse>> InvokeAsync<TService, TRequest, TResponse>(TRequest request) 
        where TService : IBusinessService<TRequest, TResponse>
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var service = Resolve<TService>();
        var result = await service.ExecuteAsync(request);

        stopwatch.Stop();
        _logger.LogInformation($"Execution of {typeof(TService).Name} took {stopwatch.ElapsedMilliseconds}ms.");

        return result;
    }

    public async Task<Result.Result> InvokeAsync<TService, TRequest>(TRequest request)
        where TService : IBusinessService<TRequest>
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var service = Resolve<TService>();
        var result = await service.ExecuteAsync(request);

        stopwatch.Stop();
        _logger.LogInformation($"Execution of {typeof(TService).Name} took {stopwatch.ElapsedMilliseconds}ms.");

        return result;
    }
    public async Task<FilterResult<TResponse>> InvokeFilterAsync<TService, TRequest, TResponse>(TRequest request) 
        where TService : IFilterService<TRequest, TResponse>
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
    
        var service = Resolve<TService>();
        var result = await service.ExecuteAsync(request);

        stopwatch.Stop();
        _logger.LogInformation($"Execution of {typeof(TService).Name} took {stopwatch.ElapsedMilliseconds}ms.");

        return result;
    }
    
    public async Task<TResult> InvokeDynamicAsync<TService, TRequest, TResult>(TRequest request) 
        where TService : IDynamicResultService<TRequest, TResult>
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
    
        var service = Resolve<TService>();
        var result = await service.ExecuteAsync(request);

        stopwatch.Stop();
        _logger.LogInformation($"Execution of {typeof(TService).Name} took {stopwatch.ElapsedMilliseconds}ms.");

        return result;
    }

    private TService Resolve<TService>()
    {
        return _serviceProvider.GetRequiredService<TService>();
    }
}