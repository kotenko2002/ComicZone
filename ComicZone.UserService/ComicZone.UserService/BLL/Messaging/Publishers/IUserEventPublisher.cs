using ComicZone.UserService.BLL.Messaging.Events;

namespace ComicZone.UserService.BLL.Messaging.Publishers
{
    public interface IUserEventPublisher
    {
        Task PublishUserCreatedAsync(UserCreatedEvent userEvent);
        Task PublishUserUpdatedAsync(UserUpdatedEvent userEvent);
    }
}
