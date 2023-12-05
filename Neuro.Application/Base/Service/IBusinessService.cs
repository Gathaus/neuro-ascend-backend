using Neuro.Application.Base.Result;

namespace Neuro.Application.Base.Service;

public interface IBusinessService<TRequest, TResponse>
{
    Task<Result<TResponse>> ExecuteAsync(TRequest request);
    
}

public interface IBusinessService<TRequest>
{
    Task<Result.Result> ExecuteAsync(TRequest request);
    
}
