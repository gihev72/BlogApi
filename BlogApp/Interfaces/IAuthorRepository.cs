using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(Guid id);
        bool AuthorExists(Guid id);
        Author GetAuthorOfABlog(Guid blogId);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
        


    }
}
