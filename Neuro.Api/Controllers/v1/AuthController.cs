using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Neuro.Application.Helpers;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using NLog.Fluent;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(BaseBusinessService baseService, IUnitOfWork unitOfWork)
    {
        _baseService = baseService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var user = new User
            {
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BloodType = model.BloodType,
                Age = model.Age,
                Address = model.Address,
                DiseaseTerm = model.DiseaseTerm,
                DiseaseLevel = model.DiseaseLevel,
                View = model.View,
                ReminderTime = model.ReminderTime,
                BeginningDate = model.BeginningDate,
                EndDate = model.EndDate,
                HavePet = model.HavePet,
                WantVirtualPet = model.WantVirtualPet,
                CountryCode = model.CountryCode,
                CountryCallingCode = model.CountryCallingCode,
                MobileNumber = model.MobileNumber,
                Amount = model.Amount,
                BeginDay = model.BeginDay,
                BeginMonth = model.BeginMonth,
                EndDay = model.EndDay,
                EndMonth = model.EndMonth,
                SelectedDays = model.SelectedDays,
                Time = model.Time,
                Usage = model.Usage,
                ImageUrl = model.ImageUrl
            };
            
            var userFromDb = _unitOfWork.Repository<User>().FindBy(x => x.Email.ToLower().Equals(model.Email.ToLower()))
                .FirstOrDefault();
            if (userFromDb != null)
            {
                return BadRequest(new {IsSuccess = false, Message = "User already exists"});
            }


            await _unitOfWork.Repository<User>().InsertAsync(user);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new {Username = user.Username, Email = user.Email,IsSuccess = true, Message = "Register Success"});
            }

            return BadRequest( new {IsSuccess = false, Message = "Register Failed"});

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var user = await _unitOfWork.Repository<User>()
                .FindBy(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
                .FirstOrDefaultAsync();
            

            if (user != null)
            {
                var medicineDays = await _unitOfWork.Repository<MedicineUser>()
                    .FindBy(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim())).ToListAsync();
                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim())) 
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date ).ToListAsync();
                return Ok(new {User = user, MedicineDays = medicineDays,UserMood = userMood.FirstOrDefault()?.Mood.ToString() ?? "None", IsSuccess = true});
            }

            return BadRequest(new {IsSuccess = false, Message = "Login Failed"});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}