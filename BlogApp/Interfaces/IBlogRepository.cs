using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IBlogRepository
    {
        ICollection<Blog> GetBlogs();
        Blog GetBlog(int id);
        bool BlogExist(int blogId);

        ICollection<Blog> GetBlogsOfAuthor(int authorId);

    }
}
