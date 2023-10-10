using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IBlogRepository
    {
        ICollection<Blog> GetBlogs();
        Blog GetBlog(Guid id);
        bool BlogExist(Guid blogId);
        ICollection<Blog> GetBlogsOfAuthor(Guid authorId);
        bool CreateBlog( Guid tagId,Guid authorId , Blog blog);
        bool UpdateBlog(Blog blog);
        bool DeleteBlog(Blog blog);
        bool Save();


    }
}
