namespace ComicZone.ComicService.DAL.Users
{
    public class CachedUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CachedAt { get; set; } = DateTime.UtcNow;
    }
}
