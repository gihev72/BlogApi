using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        //private readonly IAuthorRepository _authorRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public BlogsController(IBlogRepository blogRepository,ICommentRepository commentRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            //_authorRepository = authorRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Blog>))]
        public IActionResult GetBlogs()
        {
            var blogs = _mapper.Map<List<BlogDto>>(_blogRepository.GetBlogs());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(blogs);
        }

        [HttpGet("{blogId}")]
        [ProducesResponseType(200, Type = typeof(Blog))]
        [ProducesResponseType(400)]
        public IActionResult GetBlog(Guid blogId)
        {
            if (!_blogRepository.BlogExist(blogId))
            {
                return NotFound();
            }
            var blog = _mapper.Map<BlogDto>(_blogRepository.GetBlog(blogId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(blog);
        }

        [HttpGet("author/{authorId}")]
        [ProducesResponseType(200, Type = typeof(Blog))]
        [ProducesResponseType(400)]
        public IActionResult GetBlogOfAuthor(Guid authorId)
        {
            var blog = _mapper.Map<List<BlogDto>>(_blogRepository.GetBlogsOfAuthor(authorId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(blog);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateBlog([FromQuery] Guid authorId, [FromQuery] Guid tagId, [FromBody] CreateBlogDto blogCreate)
        {
            if(blogCreate == null)
                return BadRequest(ModelState);
            var blogMap = _mapper.Map<Blog>(blogCreate);
            // blogMap.Author = _authorRepository.GetAuthor(authorId);
            blogMap.createdAt = DateTime.Now;
            blogMap.updatedAt = DateTime.Now;

            if(!_blogRepository.CreateBlog(tagId,authorId, blogMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfuly Created");
            
        }

        [HttpPut("{blogId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBlog(Guid blogId, BlogDto updateBlog)
        {
            if(updateBlog == null) return BadRequest(ModelState);

            if(blogId != updateBlog.Id) return BadRequest(ModelState);

            if (!_blogRepository.BlogExist(blogId)) return NotFound();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var blogMap = _mapper.Map<Blog>(updateBlog);

            if (!_blogRepository.UpdateBlog(blogMap))
            {
                ModelState.AddModelError("", "Something went wrong updating blog");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{blogId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBlog(Guid blogId)
        {
            if(!_blogRepository.BlogExist(blogId)) return BadRequest(ModelState);

            var commentsToDelete = _commentRepository.GetCommentsOfABlog(blogId);
            var blogToDelete = _blogRepository.GetBlog(blogId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_commentRepository.DeleteComments(commentsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting comments");
                // return StatusCode(500, ModelState);
            }

            if(!_blogRepository.DeleteBlog(blogToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting blog");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
