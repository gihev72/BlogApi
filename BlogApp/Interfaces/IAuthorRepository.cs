using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int id);
        bool AuthorExists(int id);
        Author GetAuthorOfABlog(int blogId);
        bool CreateAuthor(Author author);
        bool Save();
        


    }
}
