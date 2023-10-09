using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        ICollection<Blog> GetBlogsByTag(int tagId);
        bool TagExist(int tagId);
        bool CreateTag(Tag tag);
        bool Save();


    }
}
