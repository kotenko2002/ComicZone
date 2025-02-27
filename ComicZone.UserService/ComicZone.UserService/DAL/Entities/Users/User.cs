using Microsoft.AspNetCore.Identity;

namespace ComicZone.UserService.DAL.Entities.Users
{
    public class User : IdentityUser
    {
        public string AvatarUrl { get; set; }
    }
}
