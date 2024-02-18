using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Neuro.Application.Helpers;
using Neuro.Application.Managers.Abstract;
using Neuro.Domain.Entities;
using Neuro.Domain.Entities.Enums;
using Neuro.Domain.UnitOfWork;
using NLog.Fluent;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class MedicineController : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;
    private readonly IUnitOfWork _unitOfWork;


    public MedicineController(BaseBusinessService baseService, IUnitOfWork unitOfWork)
    {
        _baseService = baseService;
        _unitOfWork = unitOfWork;
    }

    #endregion
    
    
}