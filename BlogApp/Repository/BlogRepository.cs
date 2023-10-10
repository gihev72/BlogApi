using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;

namespace BlogApp.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly DataContext _context;
        public BlogRepository(DataContext context)
        {
            _context = context;
        }

        public bool BlogExist(Guid blogId)
        {
           return _context.Blogs.Any(b => b.Id == blogId);
        }

        public bool CreateBlog( Guid tagId,Guid authorId , Blog blog)
        {
            var author = _context.Author.Where(a => a.Id == authorId).FirstOrDefault();
            blog.Author = author;
            var tag = _context.Tags.Where(t => t.Id == tagId).FirstOrDefault();
            var blogTag = new BlogTag()
            {
                Tag = tag,
                Blog = blog,
                
            };
            _context.BlogTags.Add(blogTag);
            _context.Blogs.Add(blog);
            return Save();
        }

        public bool DeleteBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
            return Save();
        }

        public Blog GetBlog(Guid id)
        {
            return _context.Blogs.Where(b => b.Id == id).FirstOrDefault();
        }

        public ICollection<Blog> GetBlogs()
        {
            return _context.Blogs.OrderBy(b => b.Id).ToList();
        }

        public ICollection<Blog> GetBlogsOfAuthor(Guid authorId)
        {
           return _context.Blogs.Where(b => b.Author.Id == authorId ).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBlog(Blog blog)
        {
            _context.Update(blog);
            return Save();
        }
    }
}
