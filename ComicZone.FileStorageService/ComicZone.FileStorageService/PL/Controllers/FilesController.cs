using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;
using ComicZone.FileStorageService.BLL.FileStorage;

namespace ComicZone.FileStorageService.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        public FilesController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var stream = await _fileStorage.GetFile(fileName);
            
            return File(stream, "application/octet-stream", fileName);
        }
    }
}
