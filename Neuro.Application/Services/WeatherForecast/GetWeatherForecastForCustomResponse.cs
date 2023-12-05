using Neuro.Application.Base.Service;
using Neuro.Application.Dtos;
using Neuro.Application.Responses;

namespace Neuro.Application.Services.WeatherForecast;

public class
    GetWeatherForecastForCustomResponse : IDynamicResultService<GetWeatherForecastForCustomResponse.Request,
        PagedTableResponse<WeatherForecastDto>>
{
    #region constructor

    public GetWeatherForecastForCustomResponse()
    {
    }

    #endregion

    #region Request

    public class Request
    {
    }

    #endregion

    public async Task<PagedTableResponse<WeatherForecastDto>> ExecuteAsync(Request request)
    {
        var rows = new List<WeatherForecastDto>()
        {
            new WeatherForecastDto()
            {
                Date =  DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Summary = "Summary",
                TemperatureC = 1
            },
            new WeatherForecastDto()
            {
                Date =  DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Summary = "Summary2",
                TemperatureC = 2
            }
        };
        
        
        return new PagedTableResponse<WeatherForecastDto>
        {
            IsSuccess = true,
            Rows = rows,
            Headers = new List<string> { "Name", "CountryName", "CityName", "CountyName", "PoiType" },
            PropertiesWithHeaders = new List<Header>
            {
                new Header { name = "Name", param = "Name", dataType = "string" },
            }
        };
    }
}