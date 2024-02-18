using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Base.Service;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;


    public UserController(BaseBusinessService baseService)
    {
        _baseService = baseService;
    }

    #endregion

    
}