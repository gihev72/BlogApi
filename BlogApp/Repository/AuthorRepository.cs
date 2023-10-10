using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;

namespace BlogApp.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }
        public bool AuthorExists(Guid id)
        {
            return _context.Author.Any(a => a.Id == id);
        }

        public bool CreateAuthor(Author author)
        {
            _context.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _context.Remove(author);
            return Save();
        }

        public Author GetAuthor(Guid id)
        {
            return _context.Author.Where(a => a.Id == id).FirstOrDefault();
        }

        public Author GetAuthorOfABlog(Guid blogId)
        {
            return _context.Blogs.Where(b => b.Id == blogId).Select(a => a.Author).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _context.Author.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
           _context.Update(author);
            return Save();
        }
    }
}
