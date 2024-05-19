using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Neuro.Application.Managers.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neuro.Application.Managers.Concrete
{
    public class FaceDetectionService : IFaceDetectionService
    {
        private readonly ImageAnnotatorClient _visionClient;

        public FaceDetectionService(IConfiguration configuration)
        {
            var config = configuration.GetSection("GoogleCredentials").Get<GoogleServiceConfig>();

            var googleCredentials = GoogleCredential.FromJson(JsonConvert.SerializeObject(config));

            _visionClient = new ImageAnnotatorClientBuilder
            {
                Credential = googleCredentials
            }.Build();
        }

        public async Task<FaceDetectionResult> DetectFacesAsync(IFormFile image)
        {
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            var request = new AnnotateImageRequest
            {
                Image = Image.FromBytes(memoryStream.ToArray()),
                Features = { new Feature { Type = Feature.Types.Type.FaceDetection } }
            };

            var response = await _visionClient.AnnotateAsync(request);
            var faceAnnotations = response.FaceAnnotations;

            var result = new FaceDetectionResult();

            if (faceAnnotations.Count > 0)
            {
                var face = faceAnnotations[0];
                result.Emotions = GetEmotionsFromLikelihood(face);
            }

            return result;
        }

        private List<string> GetEmotionsFromLikelihood(FaceAnnotation face)
        {
            var emotions = new List<string>();

            if (face.JoyLikelihood == Likelihood.VeryLikely || face.JoyLikelihood == Likelihood.Likely)
            {
                emotions.Add("Happy");
            }
            if (face.SorrowLikelihood == Likelihood.VeryLikely || face.SorrowLikelihood == Likelihood.Likely)
            {
                emotions.Add("Sad");
            }
            if (face.AngerLikelihood == Likelihood.VeryLikely || face.AngerLikelihood == Likelihood.Likely)
            {
                emotions.Add("Angry");
            }
            if (face.SurpriseLikelihood == Likelihood.VeryLikely || face.SurpriseLikelihood == Likelihood.Likely)
            {
                emotions.Add("Surprised");
            }

            return emotions;
        }
    }

    public class FaceDetectionResult
    {
        public List<string> Emotions { get; set; }
    }
}