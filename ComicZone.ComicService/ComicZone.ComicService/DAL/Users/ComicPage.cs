namespace ComicZone.ComicService.DAL.Users
{
    public class ComicPage
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public Comic Comic { get; set; }
        public int PageNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
