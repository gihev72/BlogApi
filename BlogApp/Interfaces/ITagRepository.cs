using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(Guid id);
        ICollection<Blog> GetBlogsByTag(Guid tagId);
        bool TagExist(Guid tagId);
        bool CreateTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
        bool Save();


    }
}
