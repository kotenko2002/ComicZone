using ComicZone.UserService.BLL.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace ComicZone.UserService.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestUploadController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;

        public TestUploadController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected or empty");
            }

            var fileName = await _fileStorage.UploadFileAsync(file);

            return Ok(new { FileName = fileName });
        }

        [HttpGet("get-link")]
        public async Task<IActionResult> GetFileLink([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("You need to pass the file name");
            }

            var link = await _fileStorage.GetFileLinkAsync(fileName);

            return Ok(new { FileLink = link });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("You need to pass the file name");
            }

            await _fileStorage.DeleteFileAsync(fileName);

            return Ok("File successfully deleted");
        }
    }
}
