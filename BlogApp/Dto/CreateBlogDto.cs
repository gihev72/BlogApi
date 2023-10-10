namespace BlogApp.Dto
{
    public class CreateBlogDto
    {
        public Guid Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string body { get; set; }
    }
}
