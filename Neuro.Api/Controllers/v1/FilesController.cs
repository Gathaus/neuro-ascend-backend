using Microsoft.AspNetCore.Mvc;
using Neuro.Api.Managers;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilesController : BaseController
    {
        private readonly StorageManager _storageManager;

        public FilesController(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string filePath, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Dosya bulunamadı.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _storageManager.UploadFileAsync(filePath, stream);
            }

            return Ok($"'{filePath}' başarıyla yüklendi.");
        }


        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var files = await _storageManager.ListFilesAsync();
            return Ok(files);
        }

        [HttpGet("download/{*filePath}")]
        public async Task<IActionResult> Download(string filePath)
        {
            var stream = await _storageManager.DownloadFileAsync(filePath);
            if (stream == null)
            {
                return NotFound();
            }

            var fileName = Path.GetFileName(filePath);
            return File(stream, "application/octet-stream", fileName);
        }


        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _storageManager.DeleteFileAsync(fileName);
            return Ok($"{fileName} başarıyla silindi.");
        }
        
        [HttpGet("info")]
        public async Task<IActionResult> GetFileInfo(string filePath)
        {
            var metadata = await _storageManager.GetFileInfoAsync(filePath);
            return Ok(metadata);
        }

        [HttpGet("directory")]
        public async Task<IActionResult> ListFilesInDirectory(string directoryPath)
        {
            var files = await _storageManager.ListFilesInDirectoryAsync(directoryPath);
            return Ok(files);
        }
        
    }
}