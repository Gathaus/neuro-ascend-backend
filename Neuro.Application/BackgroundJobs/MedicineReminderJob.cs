using Hangfire;
using Microsoft.EntityFrameworkCore;
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

    public MedicineReminderJob(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _notificationManager = new NotificationManager();
    }
    
    [Queue("critical")]
    [RecurringJob("MedicineReminderJob.cs", "0 */1 * * * *")]
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
                .FirstOrDefaultAsync();

            if (userMedicine == null || userMedicine.User?.FirebaseToken == null)
                continue;

            var timeMatch = userMedicine.Times.Any(time => Math.Abs((time.Time - currentTime).TotalMinutes) < 1);

            if (timeMatch)
                await _notificationManager.SendNotificationAsync(userMedicine.User.FirebaseToken, "Medication Time", "It's time to take your medications.");
        }
    }
}