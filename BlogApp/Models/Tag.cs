namespace BlogApp.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
