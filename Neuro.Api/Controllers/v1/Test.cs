using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Neuro.Application.Managers.Abstract;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class Test : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;


    public Test(BaseBusinessService baseService, IUnitOfWork unitOfWork, IUserService userService)
    {
        _baseService = baseService;
        _unitOfWork = unitOfWork;
        _userService = userService;
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
    
    [HttpPost("signin-google")]
    public async Task<IActionResult> VerifyGoogleToken()
    {
          // var user = await _unitOfWork.Repository<User>().FindBy(x => x.Email.Equals(mail,
            //     StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

            var mail = "rizamertyagci@gmail.com";
            var userEntity = await _unitOfWork.Repository<User>()
                .FindBy(x => x.Email.ToLower().Trim().Equals(mail.ToLower().Trim()))
                .FirstOrDefaultAsync();
            
            var userDto = new UserDto()
            {
                Id = userEntity.Id,
                FirebaseToken = userEntity.FirebaseToken,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                Age = userEntity.Age,
                AlzheimerStage = userEntity.AlzheimerStage,
                CountryCode = userEntity.CountryCode,
                MobileNumber = userEntity.MobileNumber,
                TimeZone = userEntity.TimeZone,
            };


            if (userEntity != null)
            {

                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(mail.ToLower().Trim()))
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date).ToListAsync();
                var medicinesInfo = await _userService.GetUserMedicinesAsync(userEntity.Id);
                
                return Ok(new
                {
                    User = userDto,
                    UserMood = userMood.FirstOrDefault()?.Mood.ToString() ?? "None",
                    IsSuccess = true,
                    Medicines = medicinesInfo.Medicines,
                    NextMedicines = medicinesInfo.NextMedicines,
                    ForgottenMedicines = medicinesInfo.ForgottenMedicines
                });
            }

            return BadRequest(new {IsSuccess = false, Message = "User Not Found"});

    }
}