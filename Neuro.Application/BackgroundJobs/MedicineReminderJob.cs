using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Attributes;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Application.Managers.Concrete;
using Neuro.Domain.Entities;
using Neuro.Domain.Logging;
using Neuro.Domain.UnitOfWork;
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
        var today = DateTime.UtcNow.DayOfWeek;
        var currentTime = DateTime.UtcNow.TimeOfDay;

        var medicationDays = await _unitOfWork.Repository<MedicationDay>()
            .FindBy(x => x.DayOfWeek == today)
            .ToListAsync();

        foreach (var medicationDay in medicationDays)
        {
            var userMedicine = await _unitOfWork.Repository<UserMedicine>()
                .FindBy(x => x.Id == medicationDay.UserMedicineId)
                .Include(x => x.User)
                .Include(x => x.Times)
                .Include(x=>x.Medication)
                .FirstOrDefaultAsync();

            if (userMedicine == null || userMedicine.User?.FirebaseToken == null)
                continue;

            var currentDateTime = new DateTime(1, 1, 1, currentTime.Hours, currentTime.Minutes, currentTime.Seconds);

            var timeMatch = userMedicine.Times.Any(time => time.Time.Hour == currentDateTime.Hour);

            var medicationName = userMedicine.Medication.Name;
            if (timeMatch)
                await _notificationManager.SendNotificationAsync(userMedicine.User.FirebaseToken, 
                    $"Hi {userMedicine.User.FirstName}!",
                    $"It's time to take your {medicationName} medications.");
        }
    }
}