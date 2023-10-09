using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments();
        Comment GetComment(int commentId);
        ICollection<Comment> GetCommentsOfABlog(int blogId);
        bool CommentExist(int commentId);
        bool CreateComment(Comment comment);
        bool Save();
    }
}
