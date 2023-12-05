using Neuro.Application.Base.Result;
using Neuro.Application.Base.Service;

namespace Neuro.Application.Services;

public class DeleteWeatherForecast : IBusinessService<DeleteWeatherForecast.Request, DeleteWeatherForecast.Response>
{

    #region constructor

    public DeleteWeatherForecast()
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