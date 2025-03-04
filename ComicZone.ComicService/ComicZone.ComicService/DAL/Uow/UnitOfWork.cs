using ComicZone.ComicService.DAL.Common;
using ComicZone.ComicService.DAL.Repositories.CachedUsers;

namespace ComicZone.ComicService.DAL.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private ICachedUserRepository _cachedUserRepository;
        public ICachedUserRepository CachedUserRepository => _cachedUserRepository ??= new CachedUserRepository(_context);

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
