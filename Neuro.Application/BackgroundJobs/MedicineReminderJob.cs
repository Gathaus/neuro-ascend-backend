using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Application.Managers.Abstract;
using Neuro.Application.Managers.Concrete;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Logging;

namespace Neuro.Application.BackgroundJobs;

public class MedicineReminderJob : IRecurringJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly NotificationManager _notificationManager;
    private readonly IUserService _userService;

    public MedicineReminderJob(IUnitOfWork unitOfWork, IConfiguration configuration, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _notificationManager = new NotificationManager(configuration);
    }

    public async Task Execute()
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            
        

            // Haftanın ilk günü ve saat 00:00 kontrolü
            if (utcNow.DayOfWeek == DayOfWeek.Sunday && utcNow.Hour == 0)
            {
                var medicationTimes = await _unitOfWork.Repository<MedicationTime>()
                    .FindBy(mt => mt.IsTaken)
                    .ToListAsync();

                foreach (var medicationTime in medicationTimes)
                {
                    medicationTime.IsTaken = false;
                }

                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
            }

            // Kullanıcı ve ilaç bilgilerini içeren UserMedicine nesnelerini çek
            var userMedicines = await _unitOfWork.Repository<UserMedicine>()
                .FindBy()
                .Include(um => um.User)
                .Include(um => um.MedicationTimes.Where(mt => !mt.IsTaken))
                .Include(um => um.Medication)
                .ToListAsync();

            // Gruplanmış kullanıcılar üzerinde döngü kur
            var groupedUserMedicines = userMedicines.GroupBy(um => um.User);
            foreach (var group in groupedUserMedicines)
            {
                if(utcNow.Hour == 0)
                    await _userService.CalculateUserMedicineTargetForDay(group.Key.Id);
                        
                var user = group.Key;
                if (user?.TimeZone == null || user.FirebaseToken == null)
                    continue;

                foreach (var userMedicine in group)
                {
                    var medicationDays = userMedicine.MedicationTimes
                        .Where(mt => mt.WeekDay == utcNow.DayOfWeek)
                        .ToList();

                    if (!medicationDays.Any())
                        continue;

                    var timeMatch = medicationDays.Any(mt =>
                        utcNow.Hour == mt.Time.Hours);

                    if (timeMatch)
                    {
                        var medicationName = userMedicine.Medication.Name;
                        await _notificationManager.SendNotificationAsync(user.FirebaseToken,
                            $"Hello {user.FirstName}!",
                            $"It's time to take your {medicationName} medication.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            // Hata kaydı
            await BasicNeuroLogger.LogError(e);
            throw;
        }
    }
}