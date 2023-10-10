using BlogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogTag>().HasKey(bt => new { bt.BlogId, bt.TagId });
            modelBuilder.Entity<BlogTag>()
                .HasOne(b => b.Blog)
                .WithMany(bt => bt.BlogTags)
                .HasForeignKey(t => t.BlogId);
            modelBuilder.Entity<BlogTag>()
                .HasOne(t => t.Tag)
                .WithMany(bt => bt.BlogTags)
                .HasForeignKey(b => b.TagId);
        }

    }
}
