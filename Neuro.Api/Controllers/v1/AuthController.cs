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
public class AuthController : BaseController
{
    #region constructor

    private readonly BaseBusinessService _baseService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;


    public AuthController(BaseBusinessService baseService, IUnitOfWork unitOfWork, IUserService userService)
    {
        _baseService = baseService;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    #endregion

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model == null)
        {
            return BadRequest(new {IsSuccess = false, Message = "Invalid model"});
        }

        var existingUser =
            await _unitOfWork.Repository<User>().FindBy(x => x.Email == model.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return BadRequest(new {IsSuccess = false, Message = "User already exists"});
        }

        // Modelden gelen verileri kullanarak yeni bir User nesnesi oluştur
        var user = new User
        {
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password, // Şifre güvenliği için hashleme önerilir.
            BloodType = model.BloodGroup,
            Age = model.Age,
            Address = model.Address,
            ImageUrl = model.ImageUrl ?? "test/default-user.jpg",
            AlzheimerStage = model.AlzheimerStage,
            HavePet = model.HasPet,
            WantVirtualPet = model.WantsVirtualPet,
            CountryCode = model.CountryCode,
            CountryCallingCode = model.CountryCallingCode,
            MobileNumber = model.MobileNumber,
            FirebaseToken = model.FirebaseToken,
            Diseases = model.Diseases.Select(d => new Disease {Name = d}).ToList(),
            UserMedicines = new List<UserMedicine>(),
            TimeZone = model.TimeZone
        };


        // İlaç bilgilerini User nesnesine ekle
        foreach (var med in model.Medications)
        {
            var userMedicine = new UserMedicine
            {
                Medication = new Medication {Name = med.Name},
                Usage = med.Usage,
                PillNumber = med.PillNumber,
                BeginningDate = med.BeginningDate,
                EndDate = med.EndDate,
                MedicationTimes = new List<MedicationTime>()
            };

            foreach (var medicationDay in med.MedicationDays)
            {
                foreach (var medicationTime in med.MedicationTimes)
                {
                    
                    var now = DateTime.UtcNow;
                    var userTimeSpan = TimeSpan.Parse(medicationTime.Time);
                    var minutesToAdd = (int) medicationDay.DayOfWeek * 1440; //1440 = 1 day

                    var userTimeSpanMinutes = userTimeSpan.Add(TimeSpan.FromMinutes(minutesToAdd)).TotalMinutes;
                                            
                    var timeZone = TimeZoneExtensions.GetTimeZoneInfo(user.TimeZone) ?? TimeZoneInfo.Local;
                    
                    var utcOffsetMinutes = timeZone.GetUtcOffset(now).TotalMinutes;
                    
                    var  moduleTimeSpanMinutes = ((userTimeSpanMinutes - utcOffsetMinutes)  % 10080);
                    
                    var newTimeSpan = TimeSpan.FromMinutes(moduleTimeSpanMinutes);
                    var newTimeSpanDays = newTimeSpan.Days;
                    var weekDay = (DayOfWeek) newTimeSpanDays;
                    var time = new TimeSpan(newTimeSpanDays,newTimeSpan.Hours,newTimeSpan.Minutes,0);

                    var medicationTimeData = new MedicationTime
                    {
                        Time = time,
                        WeekDay = weekDay
                    };
                    userMedicine.MedicationTimes.Add(medicationTimeData);
                }
            }


            user.UserMedicines.Add(userMedicine);
        }

        await _unitOfWork.Repository<User>().InsertAsync(user);
        var result = await _unitOfWork.SaveChangesAsync();

        var medicinesInfo = await _userService.GetUserMedicinesWithoutForgettenMedicinesAsync(user.Id);
        if (result > 0)
        {
            var userTargets = await _userService.CalculateUserTargetsAsync(user.Id);

            return Ok(new
            {
                IsSuccess = true, Message = "Registration successful",
                UserId = user.Id, Email = user.Email, User = user,
                Medicines = medicinesInfo.Medicines,
                NextMedicines = medicinesInfo.NextMedicines,
                UserTargets = userTargets

            });
        }

        return BadRequest(new {IsSuccess = false, Message = "Registration failed"});
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
                if (model.FirebaseToken != null)
                {
                    user.FirebaseToken = model.FirebaseToken;
                    _unitOfWork.Repository<User>().Update(user);
                    await _unitOfWork.SaveChangesAsync();
                }


                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date).ToListAsync();
                return Ok(new
                {
                    User = user,
                    UserMood = userMood.FirstOrDefault()?.Mood.ToString() ?? "None", IsSuccess = true
                });
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