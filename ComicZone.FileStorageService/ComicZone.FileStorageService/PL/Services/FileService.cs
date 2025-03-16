using ComicZone.FileStorageService.BLL.FileStorage;
using Grpc.Core;

namespace ComicZone.FileStorageService.PL.Services
{
    public class FileService : FileStorageService.FileStorageServiceBase
    {
        private readonly IFileStorage _fileStorage;

        public FileService(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public override async Task<UploadFileResponse> UploadFile(UploadFileRequest request, ServerCallContext context)
        {
            var ms = new MemoryStream(request.FileData.ToByteArray());
            var formFile = new FormFile(ms, 0, ms.Length, "file", request.OriginalFileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = request.ContentType
            };
            var fileName = await _fileStorage.UploadFile(formFile);
            return new UploadFileResponse { FileName = fileName };
        }

        public override async Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context)
        {
            await _fileStorage.DeleteFile(request.FileName);
            return new DeleteFileResponse { Message = "Файл видалено." };
        }
    }
}
