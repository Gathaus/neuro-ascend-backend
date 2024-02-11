using Neuro.Application.Dtos;

namespace Neuro.Application.Managers.Abstract;

public interface IUserService
{
        Task<UserMedicineResult> GetUserMedicinesAsync(int userId);
        Task<UserMedicineResult> GetUserMedicinesWithoutForgettenMedicinesAsync(int userId);


}