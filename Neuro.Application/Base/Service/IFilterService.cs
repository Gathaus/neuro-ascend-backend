using Neuro.Application.Base.Result;

namespace Neuro.Application.Base.Service;


public interface IFilterService<TRequest, TResponse>
{
    Task<FilterResult<TResponse>> ExecuteAsync(TRequest request);
}