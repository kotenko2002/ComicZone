using Microsoft.EntityFrameworkCore;
using ComicZone.ComicService.DAL.Users;

namespace ComicZone.ComicService.DAL.Common
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Comic> Comics { get; set; }
        public DbSet<ComicPage> ComicPages { get; set; }
        public DbSet<CachedUser> CachedUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comic>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.TotalPages).HasDefaultValue(0);

                entity.HasOne(e => e.Author)
                      .WithMany()
                      .HasForeignKey(e => e.AuthorId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ComicPage>(entity =>
            {
                entity.Property(e => e.PageNumber).IsRequired();
                entity.Property(e => e.ImageUrl).IsRequired();

                entity.HasOne(e => e.Comic)
                      .WithMany(e => e.Pages)
                      .HasForeignKey(e => e.ComicId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.ComicId, e.PageNumber }).IsUnique();
            });

            modelBuilder.Entity<CachedUser>(entity =>
            {
                entity.Property(e => e.Username).IsRequired();
            });
        }
    }
}
