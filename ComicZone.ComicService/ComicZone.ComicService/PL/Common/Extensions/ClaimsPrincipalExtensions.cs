using Ardalis.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ComicZone.ComicService.PL.Common.Extensions
{
    namespace BlogAPI.PL.Common.Extensions
    {
        public static class ClaimsPrincipalExtensions
        {
            public static Result<int> GetUserId(this ClaimsPrincipal principal)
            {
                var userIdString = GetInfoByDataName(principal, "userId");

                if (!userIdString.IsSuccess)
                {
                    return Result.Error(userIdString.Errors.FirstOrDefault());
                }

                if (!int.TryParse(userIdString.Value, out int userId))
                {
                    return Result.Error($"Unable to parse userId '{userIdString.Value}' to an integer.");
                }

                return Result.Success(userId);
            }

            private static Result<string> GetInfoByDataName(ClaimsPrincipal principal, string name)
            {
                var data = principal.FindFirstValue(name);

                if (data == null)
                {
                    return Result.NotFound($"No such data as {name} in Token");
                }

                return Result.Success(data);
            }
        }
    }
}
