using Microsoft.AspNetCore.Mvc;

namespace Neuro.Api.Extensions;

public static class ControllerExtensions
{
    public static string GetControllerFilePath(this ControllerBase controller, string controllerFileName)
    {
        var hostingEnvironment = (IWebHostEnvironment)controller.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
        string rootPath = hostingEnvironment.ContentRootPath;
        string controllerFolder = "Controllers"; // Controller dosyalarının bulunduğu klasör adı
        string controllerPath = Path.Combine(rootPath, controllerFolder, controllerFileName);

        return controllerPath;
    }
}