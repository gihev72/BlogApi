using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments();
        Comment GetComment(Guid commentId);
        ICollection<Comment> GetCommentsOfABlog(Guid blogId);
        bool CommentExist(Guid commentId);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);
        bool DeleteComments(List<Comment> comments);
        bool Save();
    }
}
