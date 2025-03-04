using ComicZone.ComicService.DAL.Common;
using ComicZone.ComicService.DAL.Entities;
using ComicZone.ComicService.DAL.Repositories.BaseRepository;

namespace ComicZone.ComicService.DAL.Repositories.CachedUsers
{
    public class CachedUserRepository : BaseRepository<CachedUser>, ICachedUserRepository
    {
        public CachedUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
