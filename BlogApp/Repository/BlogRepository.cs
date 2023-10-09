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

        public bool BlogExist(int blogId)
        {
           return _context.Blogs.Any(b => b.Id == blogId);
        }

        public Blog GetBlog(int id)
        {
            return _context.Blogs.Where(b => b.Id == id).FirstOrDefault();
        }

        public ICollection<Blog> GetBlogs()
        {
            return _context.Blogs.OrderBy(b => b.Id).ToList();
        }

        public ICollection<Blog> GetBlogsOfAuthor(int authorId)
        {
           return _context.Blogs.Where(b => b.Author.Id == authorId ).ToList();
        }
    }
}
