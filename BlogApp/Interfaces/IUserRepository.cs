using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        bool UserExist(int userId);
        bool CreateUser(User user);
        bool Save();


    }
}
