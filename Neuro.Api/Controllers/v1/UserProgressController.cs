using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using System;
using System.Threading.Tasks;
using Neuro.Api.Models;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserProgressController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProgressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserProgress([FromBody] UserProgress userProgress)
        {
            try
            {
                await _unitOfWork.Repository<UserProgress>().InsertAsync(userProgress);
                await _unitOfWork.SaveChangesAsync();
                return Ok(userProgress);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{userId}")]
        public async Task<IActionResult> GetUserProgress(int userId)
        {
            var userProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x => x.UserId == userId)
                .FirstOrDefaultAsync();


            if (userProgress == null)
                return NotFound();

            return Ok(userProgress);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUserProgress()
        {
            var userProgressList = await _unitOfWork.Repository<UserProgress>().FindBy().ToListAsync();
            return Ok(userProgressList);
        }

        [HttpPut("Update/{userId}")]
        public async Task<IActionResult> UpdateUserProgress(int userId, [FromBody] UserProgressDto userProgress)
        {
            try
            {
                var existingUserProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x=>x.UserId == userId).FirstOrDefaultAsync();
                if (existingUserProgress == null)
                    return NotFound();

                existingUserProgress.EveningLastFoodId = userProgress.EveningLastFoodId 
                                                         ?? existingUserProgress.EveningLastFoodId;
                
                existingUserProgress.MorningLastFoodId = userProgress.MorningLastFoodId 
                                                         ?? existingUserProgress.MorningLastFoodId;
                
                existingUserProgress.LastExerciseId = userProgress.LastExerciseId 
                                                      ?? existingUserProgress.LastExerciseId;
                
                existingUserProgress.LastActivityId = userProgress.LastActivityId 
                                                      ?? existingUserProgress.LastActivityId;
                
                existingUserProgress.LastArticleId = userProgress.LastArticleId 
                                                     ?? existingUserProgress.LastArticleId;

                _unitOfWork.Repository<UserProgress>().Update(existingUserProgress);
                await _unitOfWork.SaveChangesAsync();

                return Ok(existingUserProgress);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> DeleteUserProgress(int userId)
        {
            try
            {
                var userProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x => x.UserId == userId)
                    .FirstOrDefaultAsync();

                if (userProgress == null)
                    return NotFound();

                _unitOfWork.Repository<UserProgress>().Delete(userProgress);
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}