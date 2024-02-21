using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TargetGroupController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TargetGroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTargetGroup([FromBody] TargetGroup targetGroup)
        {
            try
            {
                await _unitOfWork.Repository<TargetGroup>().InsertAsync(targetGroup);
                await _unitOfWork.SaveChangesAsync();
                return Ok(targetGroup);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetTargetGroup(int id)
        {
            var targetGroup = await _unitOfWork.Repository<TargetGroup>().GetByIdAsync(id);

            if (targetGroup == null)
                return NotFound();

            return Ok(targetGroup);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllTargetGroups()
        {
            var targetGroups = await _unitOfWork.Repository<TargetGroup>().FindBy().ToListAsync();
            return Ok(targetGroups);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTargetGroup(int id, [FromBody] TargetGroup targetGroup)
        {
            try
            {
                var existingTargetGroup = await _unitOfWork.Repository<TargetGroup>().GetByIdAsync(id);
                if (existingTargetGroup == null)
                    return NotFound();

                existingTargetGroup.MorningFoodTarget = targetGroup.MorningFoodTarget ?? existingTargetGroup.MorningFoodTarget;
                existingTargetGroup.EveningFoodTarget = targetGroup.EveningFoodTarget ?? existingTargetGroup.EveningFoodTarget;
                existingTargetGroup.ActivityTarget = targetGroup.ActivityTarget ?? existingTargetGroup.ActivityTarget;
                existingTargetGroup.ExerciseTarget = targetGroup.ExerciseTarget ?? existingTargetGroup.ExerciseTarget;

                _unitOfWork.Repository<TargetGroup>().Update(existingTargetGroup);
                await _unitOfWork.SaveChangesAsync();

                return Ok(existingTargetGroup);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTargetGroup(int id)
        {
            try
            {
                var targetGroup = await _unitOfWork.Repository<TargetGroup>().GetByIdAsync(id);

                if (targetGroup == null)
                    return NotFound();

                _unitOfWork.Repository<TargetGroup>().Delete(targetGroup);
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
