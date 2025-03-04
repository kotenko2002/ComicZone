using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace ComicZone.UserService.BLL.Services.FileStorage
{
    public class MinIOStorage : IFileStorage
    {
        private readonly IMinioClient _client;
        private readonly string _bucketName;

        public MinIOStorage(IMinioClient client, IOptions<MinioConfig> minioConfig)
        {
            _client = client;
            _bucketName = minioConfig.Value.BucketName;

            EnsureBucketExistsAsync().GetAwaiter().GetResult();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            using (var stream = file.OpenReadStream())
            {
                long fileSize = file.Length;
                await _client.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(fileSize));
            }

            return fileName;
        }

        public async Task<string> GetFileLinkAsync(string fileName)
        {
            // Генеруємо попередньо підписаний URL дійсний протягом 1 години (3600 секунд)
            string presignedUrl = await _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithExpiry(3600));

            return presignedUrl;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _client.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));
        }

        private async Task EnsureBucketExistsAsync()
        {
            bool found = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }
    }
}
