using Microsoft.AspNetCore.Mvc;

namespace Neuro.Api.Extensions;

public static class ControllerExtensions
{
    public static byte[] GetControllerFilePath(this ControllerBase controller, string fileName)
    {
        var hostingEnvironment = (IWebHostEnvironment)controller.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
        string rootPath = hostingEnvironment.ContentRootPath;
        string filesFolder = "Files"; 
        string filePath = Path.Combine(rootPath, filesFolder, fileName);

        if (File.Exists(filePath))
        {
            // File exists, return the file as byte array
            return File.ReadAllBytes(filePath);
        }

        throw new FileNotFoundException($"The file {filePath} does not exist.");
    }

}