using Neuro.Application.Base.Result;
using Neuro.Application.Base.Service;
using Neuro.Application.Dtos;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Application.Services;

public class GetWeatherForecast : IBusinessService<GetWeatherForecast.Request, GetWeatherForecast.Response>
{

    #region constructor

    public GetWeatherForecast()
    {
    }

    #endregion
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    #region Request & Response

    public class Request
    {
    }

    public class Response
    {
        public IEnumerable<WeatherForecastDto> Forecasts { get; set; }
    }

    #endregion

    public async Task<Result<Response>> ExecuteAsync(Request request)
    {
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        var response = ToResponse(forecasts);

        return Result<Response>.Success(response);

    }
    
    private Response ToResponse(IEnumerable<WeatherForecastDto> forecasts)
    {
        return new Response()
        {
            Forecasts = forecasts
        };
    }
}