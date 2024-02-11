using Microsoft.EntityFrameworkCore;
using Neuro.Application.Dtos;
using Neuro.Application.Managers.Abstract;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserMedicineResult> GetUserMedicinesAsync(int userId)
    {
        var utcNow = DateTime.UtcNow;
        var result = new UserMedicineResult();

        var userMedicines = await _unitOfWork.Repository<UserMedicine>()
            .FindBy()
            .Where(x => x.UserId == userId)
            .Include(x => x.User)
            .Include(x => x.MedicationTimes)
            .Include(x => x.Medication)
            .ToListAsync();

        foreach (var userMedicine in userMedicines)
        {
            if (userMedicine.User?.TimeZone == null || userMedicine.User.FirebaseToken == null)
                continue;

            foreach (var medicinetime in userMedicine.MedicationTimes)
            {
                if (medicinetime.WeekDay != utcNow.DayOfWeek)
                    continue;

                var timeDifference = medicinetime.Time.Hours - utcNow.Hour;
                if (timeDifference >= 0 && timeDifference <= 1)
                {
                    result.Medicines.Add(new MedicineInfo
                    {
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber
                    });
                }
                else if (timeDifference > 1)
                {
                    var nextDayStart = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day).AddDays(1);
                    var nextDayDifference = (nextDayStart - utcNow).TotalHours;

                    if (timeDifference <= nextDayDifference)
                    {
                        result.NextMedicines.Add(new MedicineInfo
                        {
                            MedicationName = userMedicine.Medication.Name,
                            Time = medicinetime.Time,
                            PillNumber = userMedicine.PillNumber
                        });
                    }
                }
                else // Unutulan ilaçlar
                {
                    result.ForgottenMedicines.Add(new MedicineInfo
                    {
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber
                    });
                }
            }
        }

        return result;
    }
    
        public async Task<UserMedicineResult> GetUserMedicinesWithoutForgettenMedicinesAsync(int userId)
    {
        var utcNow = DateTime.UtcNow;
        var result = new UserMedicineResult();

        var userMedicines = await _unitOfWork.Repository<UserMedicine>()
            .FindBy()
            .Where(x => x.UserId == userId)
            .Include(x => x.User)
            .Include(x => x.MedicationTimes)
            .Include(x => x.Medication)
            .ToListAsync();

        foreach (var userMedicine in userMedicines)
        {
            if (userMedicine.User?.TimeZone == null || userMedicine.User.FirebaseToken == null)
                continue;

            foreach (var medicinetime in userMedicine.MedicationTimes)
            {
                if (medicinetime.WeekDay != utcNow.DayOfWeek)
                    continue;

                var timeDifference = medicinetime.Time.Hours - utcNow.Hour;
                if (timeDifference >= 0 && timeDifference <= 1)
                {
                    result.Medicines.Add(new MedicineInfo
                    {
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber
                    });
                }
                else if (timeDifference > 1)
                {
                    var nextDayStart = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day).AddDays(1);
                    var nextDayDifference = (nextDayStart - utcNow).TotalHours;

                    if (timeDifference <= nextDayDifference)
                    {
                        result.NextMedicines.Add(new MedicineInfo
                        {
                            MedicationName = userMedicine.Medication.Name,
                            Time = medicinetime.Time,
                            PillNumber = userMedicine.PillNumber
                        });
                    }
                }
            }
        }

        return result;
    }

}
