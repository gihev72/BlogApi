using System.Reflection.Metadata.Ecma335;
using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;

namespace BlogApp.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateTag(Tag tag)
        {
            _context.Add(tag);
            return Save();
        }

        public ICollection<Blog> GetBlogsByTag(int tagId)
        {
            return _context.BlogTags.Where(t => t.TagId == tagId).Select(b => b.Blog).ToList();
        }

        public Tag GetTag(int id)
        {
           return _context.Tags.Where(t => t.Id == id).FirstOrDefault();
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 
            return saved > 0 ? true : false;
        }

        public bool TagExist(int tagId)
        {
            return _context.Tags.Any(t => t.Id == tagId);
        }
    }
}
