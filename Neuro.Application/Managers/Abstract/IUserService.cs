using Neuro.Api.Models;
using Neuro.Application.Dtos;
using Neuro.Domain.Entities.Enums;

namespace Neuro.Application.Managers.Abstract;

public interface IUserService
{
        Task<UserMedicineResult> GetUserMedicinesAsync(int userId);
        Task<UserMedicineResult> GetUserMedicinesWithoutForgettenMedicinesAsync(int userId);
        Task<bool> UpdateUserTargetAsync(int userId, UserTargetTypeEnum targetType, short number = 1);

        Task<UserTargetsDto> CalculateUserTargetsAsync(int userId);
        Task<bool> CalculateUserMedicineTargetForDay(int userId);



}