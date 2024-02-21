using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Neuro.Application.Extensions;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

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
    [HttpPost("MedicineTakenList")]
    public async Task<IActionResult> MedicineTaken([FromBody] MedicineTimeTakenDto model)
    {
        var medicineTimes = await _unitOfWork.Repository<MedicationTime>()
            .FindBy(x => model.MedicineTimeIds.Contains(x.Id))
            .ToListAsync();
        Check.EntityExists(medicineTimes, "Medicine time not found");
        foreach (var medicineTime in medicineTimes)
        {
            medicineTime.IsTaken = true;
            _unitOfWork.Repository<MedicationTime>().Update(medicineTime);

        }
        await _unitOfWork.SaveChangesAsync();
        
        return Ok(new {IsSuccess = true, Message = "Medicine taken"});
    }

    #endregion
}