using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserRepository _userRepository;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IBlogRepository blogRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _blogRepository = blogRepository;
            _userRepository = userRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        public IActionResult GetComments()
        {
            var comments = _mapper.Map<List<CommentDto>>(_commentRepository.GetComments());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(comments);
        }
        [HttpGet("{commentId}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetComment(Guid commentId)
        {
            if (_commentRepository.CommentExist(commentId))
                return NotFound();
            var commnet = _mapper.Map<CommentDto>(_commentRepository.GetComment(commentId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(commnet);
        }

        [HttpGet("blog/{blogId}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentsOfABlog(Guid blogId)
        {
            var comments = _mapper.Map<List<CommentDto>>(_commentRepository.GetCommentsOfABlog(blogId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(comments);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateComment([FromQuery] Guid userId, [FromQuery] Guid blogId, [FromBody] CreateCommentDto commentCreate)
        {
            if(commentCreate == null)
                return BadRequest(ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentMap = _mapper.Map<Comment>(commentCreate);

            commentMap.Blog = _blogRepository.GetBlog(blogId);
            commentMap.User = _userRepository.GetUser(userId);
            commentMap.date = DateTime.Now;

            if (!_commentRepository.CreateComment(commentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfuly Created");
        }

        [HttpPut("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateComment(Guid commentId, [FromBody] CreateCommentDto updateComment)
        {
            if(updateComment == null)
                return BadRequest(ModelState);

            if(commentId != updateComment.Id)
                return BadRequest(ModelState);

            if (!_commentRepository.CommentExist(commentId))
                return NotFound();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentMap = _mapper.Map<Comment>(updateComment);
            commentMap.date = DateTime.Now;

            if (!_commentRepository.UpdateComment(commentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(Guid commentId)
        {
            if (!_commentRepository.CommentExist(commentId)) return NotFound();

            var commentToDelete = _commentRepository.GetComment(commentId);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (!_commentRepository.DeleteComment(commentToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("/DeleteCommentsByUser/{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCommentsByUser(Guid userId)
        {
           if(!_userRepository.UserExist(userId)) return NotFound();

           var conmmentsToDelete = _userRepository.GetCommentsByUser(userId).ToList();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_commentRepository.DeleteComments(conmmentsToDelete))
            {
                ModelState.AddModelError("", "error deleting comments");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
