namespace BlogApp.Dto
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string body { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
