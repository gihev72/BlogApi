using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
using BlogApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorRepository authorRepository,IBlogRepository blogRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public IActionResult GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>(_authorRepository.GetAuthors());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthor(Guid authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();
            var author = _mapper.Map<AuthorDto>(_authorRepository.GetAuthor(authorId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(author);
        }

        [HttpGet("blog/{blogId}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthorOABlog(Guid blogId)
        {
            if(!_blogRepository.BlogExist(blogId))
                return NotFound();
            var author = _mapper.Map<AuthorDto>(_authorRepository.GetAuthorOfABlog(blogId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(author);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAuthor([FromBody] CreateAuthorDto authorCreate)
        {
            if (authorCreate == null)
                return BadRequest(ModelState);
            var author = _authorRepository.GetAuthors().Where(a => a.UserName.Trim().ToUpper() == authorCreate.UserName.Trim().ToUpper()).FirstOrDefault();
            if (author != null)
            {
                ModelState.AddModelError("", "Author alredy exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorMap = _mapper.Map<Author>(authorCreate);
            // password should be hasshed (it will be Fixed)
            authorMap.Password = "thisisHashed";
            
            if (!_authorRepository.CreateAuthor(authorMap))
            {
                //ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok(" Successfuly Created");
        }

        [HttpPut("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAuthor(Guid authorId, [FromBody] CreateAuthorDto updateAuthor)
        {
            if (updateAuthor == null)
                return BadRequest(ModelState);

            if(authorId != updateAuthor.Id)
                return BadRequest(ModelState);

            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var authorMap = _mapper.Map<Author>(updateAuthor);

            if (!_authorRepository.UpdateAuthor(authorMap))
            {
                ModelState.AddModelError("", "Something went wrong updating author");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            if (!_authorRepository.AuthorExists(authorId)) return NotFound();

            var authorToDelete = _authorRepository.GetAuthor(authorId);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(!_authorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting author");
            }
            return NoContent();
        }

    }
}
