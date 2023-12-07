using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Base;
using Neuro.Application.Base.Service;
using Neuro.Application.Dtos;
using Neuro.Application.Responses;
using Neuro.Application.Services;
using Neuro.Application.Services.WeatherForecast;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class WeatherForecastController : BaseController
{

    #region constructor

    private readonly BaseBusinessService _baseService;


    public WeatherForecastController(BaseBusinessService baseService)
    {
        _baseService = baseService;
    }

    #endregion

    [HttpGet("GetWeatherForecast")]
    public async Task<IActionResult> GetWeatherForecast()
    {
        var response = await _baseService.InvokeAsync<GetWeatherForecast,
            GetWeatherForecast.Request, GetWeatherForecast.Response>(new());
        
        if (!response.IsSuccess)
            return BadRequest(response);
        
        return Ok(response);
    }
    
    [HttpGet("GetWeatherForecastForCustomResponse")]
    [Authorize]
    public async Task<IActionResult> GetWeatherForecastForCustomResponse()
    {
        var response = await _baseService.InvokeDynamicAsync<GetWeatherForecastForCustomResponse,
            GetWeatherForecastForCustomResponse.Request, PagedTableResponse<WeatherForecastDto>>(new ());
        
        if (!response.IsSuccess)
            return BadRequest(response);
        
        return Ok(response);
    }
}