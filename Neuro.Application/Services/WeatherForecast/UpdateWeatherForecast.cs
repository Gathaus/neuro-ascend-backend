using Neuro.Application.Base.Result;
using Neuro.Application.Base.Service;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Application.Services;

public class UpdateWeatherForecast : IBusinessService<UpdateWeatherForecast.Request, UpdateWeatherForecast.Response>
{
    #region constructor

    public UpdateWeatherForecast()
    {
    }

    #endregion

    #region Request & Response

    public class Request
    {
    }

    public class Response
    {
    }

    #endregion

    public async Task<Result<Response>> ExecuteAsync(Request request)
    {
        //TODO write business code here
        return null;
    }
}