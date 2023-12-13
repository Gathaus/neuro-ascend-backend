using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Base.Service;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class Test : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;


    public Test(BaseBusinessService baseService)
    {
        _baseService = baseService;
    }

    #endregion

    [HttpGet("Test/{id}")]
    public async Task<IActionResult> GetPoiCatalogById(int Id)
    {
        // var response = await _baseService.InvokeAsync<UpdatePoiCatalog, UpdatePoiCatalog.Request, UpdatePoiCatalog.Response>(request);
        //
        // if (!response.IsSuccess)
        //     return BadRequest(response);
        //
        // return Ok(response);
        throw new NotImplementedException();
    }
}