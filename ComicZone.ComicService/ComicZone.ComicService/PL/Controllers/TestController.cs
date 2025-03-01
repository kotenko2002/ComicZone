using ComicZone.ComicService.PL.Common.Extensions.BlogAPI.PL.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComicZone.ComicService.PL.Controllers
{
    [ApiController, Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor =  httpContextAccessor;
        }

        [HttpGet("public")]
        public ActionResult Public()
        {

            return Ok();
        }

        [HttpGet("private"), Authorize]
        public ActionResult Private()
        {
            var userIdResult = _httpContextAccessor.HttpContext.User.GetUserId();

            if (!userIdResult.IsSuccess)
            {
                return BadRequest(userIdResult.Errors);
            }

            return Ok(new { UserId = userIdResult.Value });
        }
    }
}
