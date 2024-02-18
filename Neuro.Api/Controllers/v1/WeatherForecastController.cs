using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Application.Base.Service;
using Neuro.Application.Dtos;
using Neuro.Application.Dtos.CustomResponses;
using Neuro.Application.Services.WeatherForecast;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;


namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class WeatherForecastController : BaseController
{

    #region constructor

    private readonly BaseBusinessService _baseService;
    private readonly IUnitOfWork _unitOfWork;


    public WeatherForecastController(BaseBusinessService baseService, IUnitOfWork unitOfWork)
    {
        _baseService = baseService;
        _unitOfWork = unitOfWork;
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
    
    [HttpGet("GetWeatherForecastForException")]
    public async Task<IActionResult> GetWeatherForecastForException()
    {
        throw new Exception("Test Exception");
        var response = await _baseService.InvokeDynamicAsync<GetWeatherForecastForCustomResponse,
            GetWeatherForecastForCustomResponse.Request, PagedTableResponse<WeatherForecastDto>>(new ());
        
        if (!response.IsSuccess)
            return BadRequest(response);
        
        return Ok(response);
    }
    
    [HttpGet("GetWeatherForecastFordata")]
    public async Task<IActionResult> GetWeatherForecastFordata()
    {
        var data = _unitOfWork.Repository<User>().FindBy().ToList();
        return Ok(data);
        // using(NeuroLogDbContext context = new NeuroLogDbContext())
        // {
        //     var data = await context.AdvertExtraAttributess.ToListAsync();
        //     return Ok(data);
        // }
    }
    
    [HttpPost("datatest")]
    public async Task<IActionResult> datatest()
    {
            var user = await _unitOfWork.Repository<User>().FindBy(x => x.Id == 1).FirstOrDefaultAsync();

            return Ok(user);
    }
}