using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ExerciseController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public ExerciseController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateExercise([FromBody] Exercise exercise)
    {
        try
        {
            await _unitOfWork.Repository<Exercise>().InsertAsync(exercise);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new {IsSuccess = true, Message = "Exercise created successfully."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to create exercise."});
        }
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetExercise(int id)
    {
        try
        {
            var exercise = await _unitOfWork.Repository<Exercise>().GetByIdAsync(id);
            if (exercise != null)
            {
                if (exercise.GifPath is null)
                    exercise.GifPath = "Neuro-ascend-mobil-mvp/images/photo.jpg";
                return Ok(exercise);
            }


            return NotFound(new {IsSuccess = false, Message = "Exercise not found."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to retrieve exercise."});
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllExercises()
    {
        try
        {
            var exercises = await _unitOfWork.Repository<Exercise>().FindBy().ToListAsync();

            foreach (var exercise in exercises)
            {
                if (exercise.GifPath is null)
                    exercise.GifPath = "Neuro-ascend-mobil-mvp/images/photo.jpg";
            }

            return Ok(exercises);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to retrieve exercises."});
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateExercise([FromBody] Exercise exercise)
    {
        try
        {
            _unitOfWork.Repository<Exercise>().Update(exercise);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new {IsSuccess = true, Message = "Exercise updated successfully."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to update exercise."});
        }
    }

    [HttpGet("GetUserNextExercise/{userId}")]
    public async Task<IActionResult> GetUserNextExercise(int userId)
    {
        try
        {
            var userProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (userProgress == null)
            {
                userProgress = new UserProgress
                {
                    UserId = userId,
                    EveningLastFoodId = _unitOfWork.Repository<FoodPage>().FindBy(x => x.Category.Equals("Main Course"))
                        .Min(x => x.Id),
                    MorningLastFoodId = _unitOfWork.Repository<FoodPage>().FindBy(x => x.Category.Equals("Breakfast"))
                        .Min(x => x.Id),
                    LastExerciseId = _unitOfWork.Repository<Exercise>().FindBy().Min(x => x.Id),
                    LastActivityId = _unitOfWork.Repository<Activity>().FindBy().Min(x => x.Id),
                    LastArticleId = _unitOfWork.Repository<Article>().FindBy().Min(x => x.Id)
                };

                await _unitOfWork.Repository<UserProgress>().InsertAsync(userProgress);
                await _unitOfWork.SaveChangesAsync();
            }

            var exercise = await _unitOfWork.Repository<Exercise>()
                .FindBy(x => x.Id > (userProgress.LastExerciseId ?? 0))
                .FirstOrDefaultAsync();
            if (exercise == null) return NotFound();

            if (exercise.GifPath is null)
                exercise.GifPath = "Neuro-ascend-mobil-mvp/images/photo.jpg";

            return Ok(exercise);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to retrieve exercise."});
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        try
        {
            var exercise = await _unitOfWork.Repository<Exercise>().GetByIdAsync(id);
            if (exercise != null)
            {
                _unitOfWork.Repository<Exercise>().Delete(exercise);
                await _unitOfWork.SaveChangesAsync();
                return Ok(new {IsSuccess = true, Message = "Exercise deleted successfully."});
            }

            return NotFound(new {IsSuccess = false, Message = "Exercise not found."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {IsSuccess = false, Message = "Failed to delete exercise."});
        }
    }
}