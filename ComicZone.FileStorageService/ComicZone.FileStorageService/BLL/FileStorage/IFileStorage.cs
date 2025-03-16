namespace ComicZone.FileStorageService.BLL.FileStorage
{
    public interface IFileStorage
    {
        Task<string> UploadFile(IFormFile file);
        Task<MemoryStream> GetFile(string fileName);
        Task DeleteFile(string fileName);
    }
}
