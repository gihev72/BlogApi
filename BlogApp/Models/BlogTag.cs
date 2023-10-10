namespace BlogApp.Models
{
    public class BlogTag
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public Guid TagId { get; set; }
        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}
