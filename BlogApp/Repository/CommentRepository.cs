using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;

namespace BlogApp.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }
        public bool CommentExist(Guid commentId)
        {
           return _context.Comments.Any(c => c.Id ==  commentId);
        }

        public bool CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _context.Remove(comment);
            return Save();
        }

        public bool DeleteComments(List<Comment> comments)
        {
            _context.RemoveRange(comments);
            return Save();
        }

        public Comment GetComment(Guid commentId)
        {
            return _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
        }

        public ICollection<Comment> GetComments()
        {
            return _context.Comments.ToList();
        }

        public ICollection<Comment> GetCommentsOfABlog(Guid blogId)
        {
            return _context.Comments.Where(c => c.Blog.Id ==  blogId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateComment(Comment comment)
        {
            _context.Update(comment);
            return Save();
        }
    }
}
