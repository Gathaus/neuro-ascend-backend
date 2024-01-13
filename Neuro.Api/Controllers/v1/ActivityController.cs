using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Api.Models;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class ActivityController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public ActivityController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Activity activity)
    {
        try
        {
            await _unitOfWork.Repository<Activity>().InsertAsync(activity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(activity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var activity = await _unitOfWork.Repository<Activity>().GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            if(activity.ImagePath is null)
                activity.ImagePath = "Neuro-ascend-mobil-mvp/images/photo.jpg";

            return Ok(activity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var activities = await _unitOfWork.Repository<Activity>().FindBy().ToListAsync();

            foreach (var activity in activities)
            {
                if (activity.ImagePath is null)
                    activity.ImagePath = "Neuro-ascend-mobil-mvp/images/photo.jpg";
            }
            
            return Ok(activities);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("GetUserNextActivity/{userId}")]
    public async Task<IActionResult> GetUserNextActivity(int userId)
    {
        try
        {
                    var userProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x=>x.UserId==userId).FirstOrDefaultAsync();

            if (userProgress == null)
            {
                userProgress = new UserProgress
                {
                    UserId = userId
                };
                await _unitOfWork.Repository<UserProgress>().InsertAsync(userProgress);
                await _unitOfWork.SaveChangesAsync();
            }
            var activity = await _unitOfWork.Repository<Activity>()
                .FindBy(x=>x.Id > (userProgress.LastActivityId ?? 0))
                .FirstOrDefaultAsync();
            if (activity == null) return NotFound();

            if(activity.ImagePath is null)
                activity.ImagePath = "Neuro-ascend-mobil-mvp/images/photo.jpg";
            
            return Ok(activity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Activity activity)
    {
        try
        {
            _unitOfWork.Repository<Activity>().Update(activity);
            await _unitOfWork.SaveChangesAsync();
            return Ok(activity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var activity = await _unitOfWork.Repository<Activity>().GetByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _unitOfWork.Repository<Activity>().Delete(activity);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}
