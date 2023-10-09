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
        private readonly IMapper _mapper;

        public BlogsController(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
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
        public IActionResult GetBlog(int blogId)
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
        public IActionResult GetBlogOfAuthor(int authorId)
        {
            var blog = _mapper.Map<List<BlogDto>>(_blogRepository.GetBlogsOfAuthor(authorId));
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            return Ok(blog);
        }
    }
}
