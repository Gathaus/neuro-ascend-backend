using System.Net;
using Microsoft.AspNetCore.Mvc;
using Neuro.Api.Managers;
using Neuro.Domain.Logging;

namespace Neuro.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilesController : BaseController
    {
        private readonly StorageManager _storageManager;
        private INeuroLogger _logger;


        public FilesController(StorageManager storageManager, INeuroLogger logger)
        {
            _storageManager = storageManager;
            _logger = logger;
        }
        
        public class FileModel
        {
            public string FilePath { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]FileModel model)
        {
            await _logger.LogInfo("File upload started");

            if (model.File == null || model.File.Length == 0)
            {
                await _logger.LogError(new Exception("File upload failed"), "File upload failed");
                
                return BadRequest("Dosya bulunamadı.");
            }

            using (var stream = model.File.OpenReadStream())
            {
                await _logger.LogInfo("File upload started");
                await _storageManager.UploadFileAsync(model.FilePath, stream);
            }
            await _logger.LogInfo("File upload completed");

            return Ok(new{FilePath=model.FilePath,IsSuccess=true});
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
            filePath = WebUtility.UrlDecode(filePath);
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