using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;

namespace BlogApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public User GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UserExist(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
