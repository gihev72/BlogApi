﻿namespace BlogApp.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime date { get; set; }
        public User User { get; set; }
        public Blog Blog { get; set; }
    }
}
