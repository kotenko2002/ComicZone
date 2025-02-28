using Ardalis.Result;
using ComicZone.UserService.PL.Models.Auth;

namespace ComicZone.UserService.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterRequest request);
        Task<Result<string>> LoginAsync(LoginRequest request);
    }
}
