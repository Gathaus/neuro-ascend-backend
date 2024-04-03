using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Http;
using Neuro.Application.Managers.Concrete;

namespace Neuro.Application.Managers.Abstract;

public interface IFaceDetectionService
{
    Task<FaceDetectionResult> DetectFacesAsync(IFormFile image);
}
