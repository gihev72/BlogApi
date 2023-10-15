using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if(!ModelState.IsValid) 
                {
                ModelState.AddModelError("", "error01");
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(Guid userId) 
        {
            if (!_userRepository.UserExist(userId))
                return NotFound();
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            if(!ModelState.IsValid)
                {
                ModelState.AddModelError("", "error01");
                return BadRequest(ModelState);
            }
            return Ok(user);


        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] CreateUserDto userCreate)
        {
            if(userCreate == null)
                {
                ModelState.AddModelError("", "error01");
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetUsers().Where(u => u.Email.Trim().ToUpper() == userCreate.Email.Trim().ToUpper()).FirstOrDefault();

            if(user != null)
            {
                ModelState.AddModelError("", "user with this email alredy exist!");
                return StatusCode(422, ModelState);
            }
            var userMap = _mapper.Map<User>(userCreate);

            // hashing password
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(userCreate.Password);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfuly Created");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(Guid userId, [FromBody] CreateUserDto updateUser )
        {
            if (updateUser == null)
            {
                ModelState.AddModelError("", "error01");
                return BadRequest(ModelState);
            }

            if (userId != updateUser.Id)
            {
                ModelState.AddModelError("", "error02");
                return BadRequest(ModelState);
            }

            if (!_userRepository.UserExist(userId)) return NotFound();

            if(!ModelState.IsValid) {
                ModelState.AddModelError("", "error03");
                return BadRequest(ModelState);
            }

            var userMap =_mapper.Map<User>(updateUser);

            // need password validation!!

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteUser(Guid userId)
        {
            if (!_userRepository.UserExist(userId)) return NotFound();

            var userToDelete = _userRepository.GetUser(userId);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult LoginUser([FromBody] UserLoginDto userLogin)

        {
            var foundUser = _userRepository.GetUserByUsername(userLogin.Username);
            if(foundUser == null)
                return BadRequest("Useranem or password is not correct!");

            if(!BCrypt.Net.BCrypt.Verify(userLogin.Password, foundUser.Password))
                return BadRequest("Useranem or password is not correct!");

            string Token = _userRepository.CreateToken(foundUser);
            var newRefreshToken = _userRepository.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            if (!_userRepository.SetRefreshToken(foundUser, newRefreshToken))
                return BadRequest(ModelState);

            return Ok(Token);

        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult RefreshToken([FromBody] TokenDto accessToken) 
        {
            var principal = _userRepository.GetPrincipalFromExpiredToken(accessToken.Token);
            var refreshToken = Request.Cookies["refreshToken"];

            if (principal?.Identity?.Name is null)
                return Unauthorized();

            var user = _userRepository.GetUserByUsername((principal.Identity.Name));

            if (user is null || user.RefreshToken != refreshToken || user.TokenExpires < DateTime.UtcNow)
                return Unauthorized();

            var token = _userRepository.CreateToken(user);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(token);


        }






        private void SetRefreshToken( RefreshToken refreshToken )
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,

            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }


    }
}
