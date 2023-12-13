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
            return Ok(activities);
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
