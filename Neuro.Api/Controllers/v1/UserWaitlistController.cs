using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserWaitlistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserWaitlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserWaitlist([FromBody] UserWaitlist userWaitlist)
        {
            try
            {
                await _unitOfWork.Repository<UserWaitlist>().InsertAsync(userWaitlist);
                await _unitOfWork.SaveChangesAsync();
                return Ok(userWaitlist);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserWaitlist(int id)
        {
            var userWaitlist = await _unitOfWork.Repository<UserWaitlist>().GetByIdAsync(id);
            
            if (userWaitlist == null)
                return NotFound();
            
            return Ok(userWaitlist);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUserWaitlists()
        {
            var userWaitlists = await _unitOfWork.Repository<UserWaitlist>().FindBy().ToListAsync();
            
            return Ok(userWaitlists);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUserWaitlist(int id, [FromBody] UserWaitlist userWaitlist)
        {
            try
            {
                var existingUserWaitlist = await _unitOfWork.Repository<UserWaitlist>().GetByIdAsync(id);
                if (existingUserWaitlist == null)
                    return NotFound();

                existingUserWaitlist.Email = userWaitlist.Email;

                _unitOfWork.Repository<UserWaitlist>().Update(existingUserWaitlist);
                await _unitOfWork.SaveChangesAsync();

                return Ok(existingUserWaitlist);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUserWaitlist(int id)
        {
            try
            {
                var userWaitlist = await _unitOfWork.Repository<UserWaitlist>().GetByIdAsync(id);
                if (userWaitlist == null)
                    return NotFound();

                _unitOfWork.Repository<UserWaitlist>().Delete(userWaitlist);
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] string email)
        {
            try
            {
                var existingUserWaitlist = await _unitOfWork.Repository<UserWaitlist>().FindBy(x => x.Email == email).FirstOrDefaultAsync();
                if (existingUserWaitlist != null)
                {
                    return Ok(new { message = "Email already subscribed"});
                }

                var userWaitlist = new UserWaitlist { Email = email };
                await _unitOfWork.Repository<UserWaitlist>().InsertAsync(userWaitlist);
                await _unitOfWork.SaveChangesAsync();

                var count = await _unitOfWork.Repository<UserWaitlist>().FindBy().CountAsync();
                var roundedCount = Math.Floor(count / 10.0) * 10;

                return Ok(new { message = "Email subscribed successfully", count = roundedCount });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("SubscriberCount")]
        public async Task<IActionResult> GetSubscriberCount()
        {
            try
            {
                var count = await _unitOfWork.Repository<UserWaitlist>().FindBy().CountAsync();
                var roundedCount = (int) Math.Floor(count / 10.0) * 10;
                return Ok(roundedCount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}