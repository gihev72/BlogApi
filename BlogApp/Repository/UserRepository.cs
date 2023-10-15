using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure;
using BlogApp.Data;
using BlogApp.Interfaces;
using BlogApp.Models;
using Microsoft.IdentityModel.Tokens;


namespace BlogApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:AccessToken").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var Token = new JwtSecurityToken
                (
                   claims: claims,
                   expires: DateTime.Now.AddHours(1),
                   signingCredentials: cred,
                   audience: _configuration.GetSection("AppSetting:ValidAudience").Value,
                   issuer: _configuration.GetSection("AppSetting:ValidIssuer").Value
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(Token);

            return jwt;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {

                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        public ICollection<Comment> GetCommentsByUser(Guid userId)
        {
            return _context.Comments.Where(c => c.User.Id == userId).ToList();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secret = _configuration["AppSetting:AccessToken"] ?? throw new InvalidOperationException("Secret not configured");

            var validation = new TokenValidationParameters
            {
                ValidIssuer = _configuration["AppSetting:ValidIssuer"],
                ValidAudience = _configuration["AppSetting:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        public User GetUser(Guid userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
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

        public bool SetRefreshToken(User user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.Expires;
            _context.Update(user);
            return Save();
        }

        public bool UpdateUser(User user)
        { 
            _context.Update(user);
            return Save();
        }

        public bool UserExist(Guid userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
