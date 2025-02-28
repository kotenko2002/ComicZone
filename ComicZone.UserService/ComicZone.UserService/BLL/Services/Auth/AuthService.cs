using ComicZone.UserService.DAL.Entities.Users;
using ComicZone.UserService.PL.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Ardalis.Result;

namespace ComicZone.UserService.BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _config;
        private readonly UserManager<User> _userManager;

        public AuthService(
            IOptions<JwtConfig> jwtOptions,
            UserManager<User> userManager)
        {
            _config = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            User existsUser = await _userManager.FindByNameAsync(request.Username);
            if (existsUser != null)
            {
                return Result.Invalid(new List<ValidationError> {
                    new ValidationError($"A user with the name {request.Username} already exists!")
                });
            }

            var user = new User()
            {
                UserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result.Invalid(result.Errors.Select(e => new ValidationError(e.Description)).ToList());
            }

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
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id),
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
