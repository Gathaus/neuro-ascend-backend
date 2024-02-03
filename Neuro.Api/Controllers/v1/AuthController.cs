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
            ImageUrl = model.ImageUrl,
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
                Days = med.MedicationDays.Select(d => new MedicationDay {DayOfWeek = d.DayOfWeek}).ToList(),
                Times = med.MedicationTimes.Select(t =>
                {
                    var parts = t.Time.Split('|');
                    if (parts.Length > 1)
                    {
                        var dateTimeString = parts[0];
                        var timezoneId = parts[1];

                        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                        var offset = timeZoneInfo.GetUtcOffset(DateTime.UtcNow);

                        DateTime.TryParse(dateTimeString, out var dateTime);
                
                        // Zaman dilimi bilgisini kullanarak DateTimeOffset oluştur
                        var dateTimeOffset = new DateTimeOffset(dateTime, offset);

                        // PostgreSQL'e yazmadan önce UTC'ye dönüştür
                        var utcDateTimeOffset = dateTimeOffset.ToUniversalTime();
                
                        return new TimeOfDay { Time = utcDateTimeOffset };
                    }

                    throw new ArgumentException("Time information is incomplete in the TimeOfDayModel.");
                }).ToList()
            };
            user.UserMedicines.Add(userMedicine);
        }

        await _unitOfWork.Repository<User>().InsertAsync(user);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result > 0)
        {
            return Ok(new
            {
                IsSuccess = true, Message = "Registration successful",
                UserId = user.Id, Email = user.Email, User = user,
                UserMood = "None",
                MedicineDays = model.Medications.SelectMany(x => x.MedicationDays).Select(x => x.DayOfWeek).ToList()
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

                var medicineDays = await _unitOfWork.Repository<MedicationDay>()
                    .FindBy(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
                    .Select(x => x.DayOfWeek)
                    .ToListAsync();

                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date).ToListAsync();
                return Ok(new
                {
                    User = user, MedicineDays = medicineDays,
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

    private DateTimeOffset ParseDateTimeOffset(string dateTimeOffsetString)
    {
        var parts = dateTimeOffsetString.Split('|');
        if (parts.Length != 3) throw new ArgumentException("Invalid DateTimeOffset format.");

        var dateTime = DateTime.Parse(parts[0]);
        var offset = TimeSpan.FromMinutes(int.Parse(parts[1]));
        return new DateTimeOffset(dateTime, offset);
    }
}