using Microsoft.AspNetCore.Mvc;
using Neuro.Application.Managers.Abstract;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FaceDetectionController : BaseController
    {
        private readonly IFaceDetectionService _faceDetectionService;

        public FaceDetectionController(IFaceDetectionService faceDetectionService)
        {
            _faceDetectionService = faceDetectionService;
        }

        [HttpPost]
        public async Task<IActionResult> DetectFaces(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image provided.");

            var faceEmotion = await _faceDetectionService.DetectFacesAsync(image);
            return Ok(faceEmotion);
        }
    }
}