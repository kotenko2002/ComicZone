namespace ComicZone.ComicService.DAL.Users
{
    public class Comic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public CachedUser Author { get; set; }
        public string CoverImageUrl { get; set; }
        public int TotalPages { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ComicPage> Pages { get; set; } = new List<ComicPage>();
    }
}
