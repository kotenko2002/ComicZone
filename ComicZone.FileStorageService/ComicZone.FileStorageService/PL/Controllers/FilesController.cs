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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не надано або він порожній.");

            var fileName = await _fileStorage.UploadFile(file);
            
            return Ok(new { FileName = fileName });
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var stream = await _fileStorage.GetFile(fileName);
            
            return File(stream, "application/octet-stream", fileName);
        }

        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await _fileStorage.DeleteFile(fileName);
            
            return Ok("Файл видалено.");
        }
    }
}
