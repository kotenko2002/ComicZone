using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace ComicZone.FileStorageService.BLL.FileStorage
{

    public class MinIOStorage : IFileStorage
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioConfig _minioConfig;

        public MinIOStorage(IMinioClient minioClient, IOptions<MinioConfig> minioConfig)
        {
            _minioClient = minioClient;
            _minioConfig = minioConfig.Value;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_minioConfig.BucketName));
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_minioConfig.BucketName));
            }

            using (var stream = file.OpenReadStream())
            {
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_minioConfig.BucketName)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType));
            }

            return fileName;
        }

        public async Task<MemoryStream> GetFile(string fileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_minioConfig.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => { stream.CopyTo(memoryStream); }));
            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task DeleteFile(string fileName)
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_minioConfig.BucketName)
                .WithObject(fileName));
        }
    }
}
