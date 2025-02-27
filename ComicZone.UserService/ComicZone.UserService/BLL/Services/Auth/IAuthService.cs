using ComicZone.UserService.PL.Models.Auth;

namespace ComicZone.UserService.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}
