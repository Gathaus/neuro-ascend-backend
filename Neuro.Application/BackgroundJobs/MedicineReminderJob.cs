using System.Globalization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Application.Managers.Concrete;
using Neuro.Domain.Entities;
using Neuro.Domain.Logging;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Logging;
using NLog.Fluent;

namespace Neuro.Application.BackgroundJobs;

public class MedicineReminderJob : IRecurringJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly NotificationManager _notificationManager;

    public MedicineReminderJob(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _notificationManager = new NotificationManager(configuration);
    }

    [Queue("critical")]
    [RecurringJob("MedicineReminderJob.cs", "0 0 */1 * * *")]
    public async Task Execute()
    {
        try
        {
            var utcNow = DateTime.UtcNow;
        
            // Tüm UserMedicine nesnelerini al
            var userMedicines = await _unitOfWork.Repository<UserMedicine>()
                .FindBy()
                .Include(x => x.User)
                .Include(x => x.MedicationTimes)
                .Include(x => x.Medication)
                .ToListAsync();
        
            foreach (var userMedicine in userMedicines)
            {
                if (userMedicine.User?.TimeZone == null || userMedicine.User.FirebaseToken == null)
                    continue;
                
                // Kullanıcının yerel gününe göre ilaç günlerini filtrele
                var medicationDays = userMedicine.MedicationTimes
                    .Where(x => x.WeekDay == utcNow.DayOfWeek)
                    .ToList();
        
                if (!medicationDays.Any())
                    continue;
        
                var timeMatch = userMedicine.MedicationTimes.Any(time =>
                    utcNow.Hour == time.Time.Hours);
                
                await BasicNeuroLogger.LogInfo("Sunucu zamanı: " + utcNow.ToString(CultureInfo.InvariantCulture) +
                                               " Kullanıcı zamanı: " +
                                               utcNow.ToString(CultureInfo.InvariantCulture) +
                                               " Kullanıcı Timezone: " +
                                               userMedicine.UserId);
                
                var medicationName = userMedicine.Medication.Name;
                if (timeMatch)
                    await _notificationManager.SendNotificationAsync(userMedicine.User.FirebaseToken,
                        $"Hi {userMedicine.User.FirstName}!",
                        $"It's time to take your {medicationName} medications.");
            }
        }
        catch (Exception e)
        {
            await BasicNeuroLogger.LogError(e);
            throw;
        }
    }

}