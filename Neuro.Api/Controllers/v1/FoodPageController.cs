using Microsoft.AspNetCore.Mvc;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class FoodPageController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public FoodPageController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] FoodPage foodPage)
    {
        await _unitOfWork.Repository<FoodPage>().InsertAsync(foodPage);
        await _unitOfWork.SaveChangesAsync();
        return Ok(foodPage);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var foodPage = await _unitOfWork.Repository<FoodPage>().GetByIdAsync(id);
        if (foodPage == null) return NotFound();
        return Ok(foodPage);
    }

    [HttpGet("List")]
    public IActionResult List()
    {
        var foodPages = _unitOfWork.Repository<FoodPage>().FindBy();
        return Ok(foodPages);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FoodPage updatedFoodPage)
    {
        var foodPage = await _unitOfWork.Repository<FoodPage>().GetByIdAsync(id);
        if (foodPage == null) return NotFound();


        _unitOfWork.Repository<FoodPage>().Update(foodPage);
        await _unitOfWork.SaveChangesAsync();
        return Ok(foodPage);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var foodPage = await _unitOfWork.Repository<FoodPage>().GetByIdAsync(id);
        if (foodPage == null) return NotFound();

        _unitOfWork.Repository<FoodPage>().Delete(foodPage);
        await _unitOfWork.SaveChangesAsync();
        return Ok();
    }
}
