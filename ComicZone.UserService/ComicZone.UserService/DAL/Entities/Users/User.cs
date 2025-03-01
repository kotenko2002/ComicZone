using Microsoft.AspNetCore.Identity;

namespace ComicZone.UserService.DAL.Entities.Users
{
    public class User : IdentityUser<int>
    {
        public string AvatarUrl { get; set; }
    }
}
