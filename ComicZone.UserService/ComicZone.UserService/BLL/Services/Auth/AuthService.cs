using ComicZone.UserService.DAL.Entities.Users;
using ComicZone.UserService.PL.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Ardalis.Result;
using ComicZone.UserService.BLL.Messaging.Publishers;
using ComicZone.UserService.BLL.Messaging.Events;

namespace ComicZone.UserService.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _config;
        private readonly UserManager<User> _userManager;
        private readonly IUserEventPublisher _userEventPublisher;

        public AuthService(
            IOptions<JwtConfig> jwtOptions,
            UserManager<User> userManager,
            IUserEventPublisher userEventPublisher)
        {
            _config = jwtOptions.Value;
            _userManager = userManager;
            _userEventPublisher = userEventPublisher;
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            User existsUser = await _userManager.FindByNameAsync(request.Username);
            if (existsUser != null)
            {
                return Result.Conflict($"A user with the name {request.Username} already exists!");
            }

            var user = new User()
            {
                UserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result.Error(new ErrorList(result.Errors.Select(e => e.Description)));
            }

            var userCreatedEvent = new UserCreatedEvent
            {
                Id = user.Id,
                Username = user.UserName,
                AvatarUrl = null // temp
            };

            await _userEventPublisher.PublishUserCreatedAsync(userCreatedEvent);

            return Result.Success();
        }

        public async Task<Result<string>> LoginAsync(LoginRequest request)
        {
            User user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Result.Unauthorized("Incorrect username or password");
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            JwtSecurityToken accessToken = GenerateAccessToken(authClaims);

            return Result.Success(new JwtSecurityTokenHandler().WriteToken(accessToken));
        }

        private JwtSecurityToken GenerateAccessToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

            return new JwtSecurityToken(
                issuer: _config.ValidIssuer,
                audience: _config.ValidAudience,
                expires: DateTime.Now.AddDays(_config.TokenValidityInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
