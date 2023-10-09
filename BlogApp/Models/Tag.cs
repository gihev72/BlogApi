namespace BlogApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; }
    }
}
