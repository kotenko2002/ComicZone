using ComicZone.ComicService.DAL.Repositories.CachedUsers;

namespace ComicZone.ComicService.DAL.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        ICachedUserRepository CachedUserRepository { get; }
        Task CommitAsync();
    }
}
