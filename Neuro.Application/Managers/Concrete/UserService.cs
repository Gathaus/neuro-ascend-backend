using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Application.Dtos;
using Neuro.Application.Extensions;
using Neuro.Application.Managers.Abstract;
using Neuro.Domain.Entities;
using Neuro.Domain.Entities.Enums;
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
                        Id = medicinetime.Id,
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber,
                        Usage = userMedicine.Usage
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
                            Id = medicinetime.Id,
                            MedicationName = userMedicine.Medication.Name,
                            Time = medicinetime.Time,
                            PillNumber = userMedicine.PillNumber,
                            Usage = userMedicine.Usage
                        });
                    }
                }
                else // Unutulan ilaçlar
                {
                    result.ForgottenMedicines.Add(new MedicineInfo
                    {
                        Id = medicinetime.Id,
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber,
                        Usage = userMedicine.Usage
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
                        Id = medicinetime.Id,
                        MedicationName = userMedicine.Medication.Name,
                        Time = medicinetime.Time,
                        PillNumber = userMedicine.PillNumber,
                        Usage = userMedicine.Usage
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
                            Id = medicinetime.Id,
                            MedicationName = userMedicine.Medication.Name,
                            Time = medicinetime.Time,
                            PillNumber = userMedicine.PillNumber,
                            Usage = userMedicine.Usage
                        });
                    }
                }
            }
        }

        return result;
    }

    public async Task<bool> UpdateUserTargetAsync(int userId, UserTargetTypeEnum targetType, short number = 1)
    {
        try
        {
            var date = DateTime.UtcNow;
            var userTarget = await _unitOfWork.Repository<UserTarget>()
                .FindBy(x => x.UserId == userId
                             && x.CreatedDate.Date == date.Date).FirstOrDefaultAsync();

            if (userTarget == null)
            {
                var lastUserTarget = await _unitOfWork.Repository<UserTarget>()
                    .FindBy(x => x.UserId == userId)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                int targetGroupId;
                if (lastUserTarget != null)
                {
                    targetGroupId = lastUserTarget.TargetGroupId;
                }
                else
                {
                    // If user didn't have any target before, get the first target group id from the database
                    //TODO targetgroup must be decided while registiring and save it to UserEntity
                    targetGroupId = await _unitOfWork.Repository<TargetGroup>()
                        .FindBy()
                        .OrderBy(x => x.Id)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();
                }

                var newUserTarget = new UserTarget
                {
                    UserId = userId,
                    TargetGroupId = targetGroupId
                };
                switch (targetType)
                {
                    case UserTargetTypeEnum.Activity:
                        newUserTarget.ActivityDone += number;
                        break;
                    case UserTargetTypeEnum.Exercise:
                        newUserTarget.ExerciseDone += number;
                        break;
                    case UserTargetTypeEnum.Medicine:
                        newUserTarget.MedicineTaken += number;
                        break;
                    case UserTargetTypeEnum.MorningFood:
                        newUserTarget.MorningFoodTaken += number;
                        break;
                    case UserTargetTypeEnum.EveningFood:
                        newUserTarget.EveningFoodTaken += number;
                        break;
                    case UserTargetTypeEnum.Article:
                        newUserTarget.ArticleDone += number;
                        break;
                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                switch (targetType)
                {
                    case UserTargetTypeEnum.Activity:
                        userTarget.ActivityDone += number;
                        break;
                    case UserTargetTypeEnum.Exercise:
                        userTarget.ExerciseDone += number;
                        break;
                    case UserTargetTypeEnum.Medicine:
                        userTarget.MedicineTaken += number;
                        break;
                    case UserTargetTypeEnum.MorningFood:
                        userTarget.MorningFoodTaken += number;
                        break;
                    case UserTargetTypeEnum.EveningFood:
                        userTarget.EveningFoodTaken += number;
                        break;
                    case UserTargetTypeEnum.Article:
                        userTarget.ArticleDone += number;
                        break;
                }

                _unitOfWork.Repository<UserTarget>().Update(userTarget);
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<UserTargetsDto> CalculateUserTargetsAsync(int userId)
    {
        var userTarget = await _unitOfWork.Repository<UserTarget>()
            .FindBy(x => x.UserId == userId && x.CreatedDate.Date == DateTime.UtcNow.Date)
            .SingleOrDefaultAsync();

        if (userTarget == null)
            return new UserTargetsDto();

        var targetGroup = await _unitOfWork.Repository<TargetGroup>()
            .FindBy(x => x.Id == userTarget.TargetGroupId)
            .FirstOrDefaultAsync();

        if (targetGroup == null)
        {
            return new UserTargetsDto();
        }
        
        
        var totalFoodTarget = (targetGroup.MorningFoodTarget + targetGroup.EveningFoodTarget) ?? Decimal.Zero;
        var totalFoodTaken = (userTarget.MorningFoodTaken + userTarget.EveningFoodTaken) ?? Decimal.Zero;
        var totalExerciseAndActivity = (userTarget.ExerciseDone + userTarget.ActivityDone) ?? Decimal.Zero;
        var totalExerciseAndActivityTarget = (targetGroup.ExerciseTarget + targetGroup.ActivityTarget) ?? Decimal.Zero;

        var userTargetsDto = new UserTargetsDto
        {
            Medicine = NumericExtensions.Percentage(userTarget.MedicineTaken ?? Decimal.Zero,
                targetGroup.MedicineTarget ?? Decimal.Zero, 0, 100, 35),
            Food = NumericExtensions.Percentage(totalFoodTaken, totalFoodTarget,0,100,35),
            Exercise =  NumericExtensions.Percentage(totalExerciseAndActivity, totalExerciseAndActivityTarget,0,100,35)
        };

        return userTargetsDto;
    }
}