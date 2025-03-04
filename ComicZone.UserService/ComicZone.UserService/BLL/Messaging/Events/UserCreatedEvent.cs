namespace ComicZone.UserService.BLL.Messaging.Events
{
    public class UserCreatedEvent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
    }
}
