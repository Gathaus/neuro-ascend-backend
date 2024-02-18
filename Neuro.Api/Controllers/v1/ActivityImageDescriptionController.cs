using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

// Eğer ActivityImageDescription için DTO kullanılacaksa bu kütüphaneyi ekleyin.

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActivityImageDescriptionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActivityImageDescriptionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateActivityImageDescription([FromBody] ActivityImageDescription activityImageDescription)
        {
            try
            {
                await _unitOfWork.Repository<ActivityImageDescription>().InsertAsync(activityImageDescription);
                await _unitOfWork.SaveChangesAsync();
                return Ok(activityImageDescription);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{activityId}")]
        public async Task<IActionResult> GetActivityImageDescription(int activityId)
        {
            var activityImageDescription = await _unitOfWork.Repository<ActivityImageDescription>()
                .FindBy(x=>x.ActivityId == activityId)
                .OrderBy(x=>x.Order)
                .ToListAsync();

            if (activityImageDescription == null)
                return NotFound();

            return Ok(activityImageDescription);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllActivityImageDescriptions()
        {
            var activityImageDescriptions = await _unitOfWork.Repository<ActivityImageDescription>().FindBy().ToListAsync();
            return Ok(activityImageDescriptions);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateActivityImageDescription(int id, [FromBody] ActivityImageDescription activityImageDescription)
        {
            try
            {
                var existingActivityImageDescription = await _unitOfWork.Repository<ActivityImageDescription>().GetByIdAsync(id);
                if (existingActivityImageDescription == null)
                    return NotFound();

                existingActivityImageDescription.ActivityId = activityImageDescription.ActivityId;
                existingActivityImageDescription.Order = activityImageDescription.Order;
                existingActivityImageDescription.Description = activityImageDescription.Description ?? existingActivityImageDescription.Description;
                existingActivityImageDescription.ImageUrl = activityImageDescription.ImageUrl ?? existingActivityImageDescription.ImageUrl;

                _unitOfWork.Repository<ActivityImageDescription>().Update(existingActivityImageDescription);
                await _unitOfWork.SaveChangesAsync();

                return Ok(existingActivityImageDescription);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteActivityImageDescription(int id)
        {
            try
            {
                var activityImageDescription = await _unitOfWork.Repository<ActivityImageDescription>().GetByIdAsync(id);

                if (activityImageDescription == null)
                    return NotFound();

                _unitOfWork.Repository<ActivityImageDescription>().Delete(activityImageDescription);
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
