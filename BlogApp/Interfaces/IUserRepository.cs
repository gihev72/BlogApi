using System.Security.Claims;
using BlogApp.Models;

namespace BlogApp.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(Guid userId);
        User GetUserByUsername(string username);
        ICollection<Comment> GetCommentsByUser(Guid userId);
        bool UserExist(Guid userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        string CreateToken(User user);
        RefreshToken GenerateRefreshToken();
        bool SetRefreshToken(User user,RefreshToken refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken (string  token);


    }
}
