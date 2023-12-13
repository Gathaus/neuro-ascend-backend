using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;
using Neuro.Domain.UnitOfWork;
using System;
using System.Threading.Tasks;
using Neuro.Api.Extensions;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ArticleController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateArticle([FromBody] Article article)
        {
            try
            {
                await _unitOfWork.Repository<Article>().InsertAsync(article);
                await _unitOfWork.SaveChangesAsync();
                return Ok(article);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
            if (article == null)
                return NotFound();
            return Ok(article);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _unitOfWork.Repository<Article>().FindBy().ToListAsync();
            return Ok(articles);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] Article article)
        {
            try
            {
                var existingArticle = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
                if (existingArticle == null)
                    return NotFound();

                existingArticle.Title = article.Title;
                existingArticle.Text = article.Text;
                existingArticle.ArticleImagePath = article.ArticleImagePath;
                existingArticle.AuthorImagePath = article.AuthorImagePath;

                _unitOfWork.Repository<Article>().Update(existingArticle);
                await _unitOfWork.SaveChangesAsync();

                return Ok(existingArticle);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
                if (article == null)
                    return NotFound();

                _unitOfWork.Repository<Article>().Delete(article);
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("GetControllerPath")]
        public IActionResult GetControllerPath()
        {
            string controllerFileName = "ArticleController.cs";
            string controllerPath = this.GetControllerFilePath(controllerFileName);

            return Ok(controllerPath);
        }
    }
}
