using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
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
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId) 
        {
            if (!_userRepository.UserExist(userId))
                return NotFound();
            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(user);


        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] CreateUserDto userCreate)
        {
            if(userCreate == null)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers().Where(u => u.Email.Trim().ToUpper() == userCreate.Email.Trim().ToUpper()).FirstOrDefault();

            if(user != null)
            {
                ModelState.AddModelError("", "user with this email alredy exist!");
                return StatusCode(422, ModelState);
            }
            var userMap = _mapper.Map<User>(userCreate);
            // Password should be Hashed.
            userMap.Password = "hashedPassword";
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfuly Created");
        }


    }
}
