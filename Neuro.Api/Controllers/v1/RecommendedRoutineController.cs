using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.UnitOfWork;
using Neuro.Domain.Entities; 
using Neuro.Api.Models;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class RecommendedRoutineController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public RecommendedRoutineController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // POST: api/v1/RecommendedRoutine
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RecommendedRoutine model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _unitOfWork.Repository<RecommendedRoutine>().InsertAsync(model);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }

    // GET: api/v1/RecommendedRoutine/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var routine = await _unitOfWork.Repository<RecommendedRoutine>().GetByIdAsync(id);
        if (routine == null)
        {
            return NotFound();
        }

        return Ok(routine);
    }

    // GET: api/v1/RecommendedRoutine
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var routines = await _unitOfWork.Repository<RecommendedRoutine>().FindBy().ToListAsync();
        return Ok(routines);
    }

    // PUT: api/v1/RecommendedRoutine/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RecommendedRoutine model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        _unitOfWork.Repository<RecommendedRoutine>().Update(model);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/v1/RecommendedRoutine/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var routine = await _unitOfWork.Repository<RecommendedRoutine>().GetByIdAsync(id);
        if (routine == null)
        {
            return NotFound();
        }

        _unitOfWork.Repository<RecommendedRoutine>().Delete(routine);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}
