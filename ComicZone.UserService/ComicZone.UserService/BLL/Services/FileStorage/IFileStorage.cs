namespace ComicZone.UserService.BLL.Services.FileStorage
{
    public interface IFileStorage
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> GetFileLinkAsync(string fileId);
        Task DeleteFileAsync(string fileId);
    }
}
