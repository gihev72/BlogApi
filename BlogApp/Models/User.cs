namespace BlogApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? profileImgUrl { get; set; }
        public string Username { get; set; }
        public string Role { get; set; } = "user";
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpires { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
    }
}
