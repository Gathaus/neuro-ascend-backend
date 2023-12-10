using Neuro.Application.Base.Result;
using Neuro.Application.Base.Service;

namespace Neuro.Application.Services.WeatherForecast;

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