using Amazon.S3;
using Amazon.S3.Model;

namespace Neuro.Api.Managers
{
    public class StorageManager
    {
        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "neuro-ascend-blob-newstorage";

        public StorageManager(string accessKey, string secretKey)
        {
            _s3Client = new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
            {
                ServiceURL = "https://nyc3.digitaloceanspaces.com",
                ForcePathStyle = true
            });
        }

        public StorageManager(AmazonS3Config accessKey)
        {
            throw new NotImplementedException();
        }

        public async Task UploadFileAsync(string filePath, Stream fileStream)
        {
            var putRequest = new PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = BucketName,
                Key = filePath 
            };

            await _s3Client.PutObjectAsync(putRequest);
        }


        public async Task<IEnumerable<string>> ListFilesAsync()
        {
            var listRequest = new ListObjectsRequest
            {
                BucketName = BucketName
            };

            var response = await _s3Client.ListObjectsAsync(listRequest);
            var fileNames = new List<string>();

            foreach (var obj in response.S3Objects)
            {
                fileNames.Add(obj.Key);
            }

            return fileNames;
        }

        public async Task<Stream> DownloadFileAsync(string filePath)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = filePath // Tam yol bilgisi
            };

            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }

        public async Task DeleteFileAsync(string filePath)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = filePath // Tam yol bilgisi
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
        public async Task<MetadataCollection> GetFileInfoAsync(string filePath)
        {
            var getRequest = new GetObjectMetadataRequest
            {
                BucketName = BucketName,
                Key = filePath
            };

            var response = await _s3Client.GetObjectMetadataAsync(getRequest);
            return response.Metadata;
        }
        public async Task<IEnumerable<string>> ListFilesInDirectoryAsync(string directoryPath)
        {
            var listRequest = new ListObjectsRequest
            {
                BucketName = BucketName,
                Prefix = directoryPath.EndsWith("/") ? directoryPath : directoryPath + "/"
            };

            var response = await _s3Client.ListObjectsAsync(listRequest);
            return response.S3Objects.Select(o => o.Key);
        }

    }
}
