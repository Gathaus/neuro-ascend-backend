using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Application.Managers.Abstract;
using Neuro.Domain.Entities;
using Neuro.Domain.Entities.Enums;
using Neuro.Domain.UnitOfWork;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class FoodPageController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public FoodPageController(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
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

        
        var recommendedDatas = await _unitOfWork.Repository<FoodPage>()
            .FindBy(x => foodPage.RecommendedRecipes.Contains(x.Id))
            .Select(x => new {x.Id, x.Name, x.ImagePath}).ToListAsync();


        return Ok(new{ foodData= foodPage, recommendedDatas});
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
        var sss = await _unitOfWork.Repository<UserProgress>().FindBy().ToListAsync();
        var userProgress = await _unitOfWork.Repository<UserProgress>().FindBy(x=>x.UserId==userId).FirstOrDefaultAsync();
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
            
            if(isMorning)
                await _userService.UpdateUserTargetAsync(userId, UserTargetTypeEnum.MorningFood);
            else
                await _userService.UpdateUserTargetAsync(userId, UserTargetTypeEnum.EveningFood);


        }

        var foodPageQuery = isMorning
            ? _unitOfWork.Repository<FoodPage>()
                .FindBy(x => x.Id > (userProgress.MorningLastFoodId ?? 0) &&
                             x.Category.Equals("Breakfast"))
                .OrderBy(x => x.Id)

            : _unitOfWork.Repository<FoodPage>()
                .FindBy(x => x.Id > (userProgress.EveningLastFoodId ?? 0)
                             && x.Category.Equals("Main Course"))
                .OrderBy(x => x.Id);

        
        
        var foodPage = await foodPageQuery.FirstOrDefaultAsync();
        if (foodPage == null) return NotFound();


        if (foodPage.VideoPath is null)
            foodPage.VideoPath = "Neuro-ascend-mobil-mvp/videos/SampleVideo";
        return Ok(new {foodData= foodPage});
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