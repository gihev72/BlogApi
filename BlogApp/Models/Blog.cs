namespace BlogApp.Models
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string body { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public ICollection<Comment>? comments { get; set; }
        public User Author { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }

    }
}
