using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        if (foodPage.VideoPath is null)
            foodPage.VideoPath = "Neuro-ascend-mobil-mvp/videos/SampleVideo";


        return Ok(foodPage);
    }

    [HttpGet("List")]
    public IActionResult List()
    {
        var foodPages = _unitOfWork.Repository<FoodPage>().FindBy();

        foreach (var foodPage in foodPages)
        {
            if (foodPage.VideoPath is null)
                foodPage.VideoPath = "Neuro-ascend-mobil-mvp/videos/SampleVideo";
        }

        return Ok(foodPages);
    }

    [HttpGet("GetUserNextFoodPage/{userId}")]
    public async Task<IActionResult> GetUserNextFoodPage(int userId, bool isMorning = false)
    {
        var userProgress = await _unitOfWork.Repository<UserProgress>().GetByIdAsync(userId);
        if (userProgress == null)
        {
            userProgress = new UserProgress
            {
                UserId = userId
            };
            await _unitOfWork.Repository<UserProgress>().InsertAsync(userProgress);
            await _unitOfWork.SaveChangesAsync();
        }

        var foodPageQuery = isMorning
            ? _unitOfWork.Repository<FoodPage>()
                .FindBy(x => x.Id == (userProgress.MorningLastFoodId ?? 1) && 
                             x.Category.Equals("Breakfast"))
            : _unitOfWork.Repository<FoodPage>()
                .FindBy(x => x.Id == (userProgress.EveningLastFoodId ?? 1)
                             && x.Category.Equals("Main Course"));

        var foodPage = await foodPageQuery.FirstOrDefaultAsync();
        if (foodPage == null) return NotFound();


        if (foodPage.VideoPath is null)
            foodPage.VideoPath = "Neuro-ascend-mobil-mvp/videos/SampleVideo";


        return Ok(foodPage);
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