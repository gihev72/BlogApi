﻿using AutoMapper;
using BlogApp.Dto;
using BlogApp.Interfaces;
using BlogApp.Models;
using BlogApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TagDto>))]
        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(tags);
        }

        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTag(Guid tagId)
        {
            if (!_tagRepository.TagExist(tagId))
            {
                return NotFound();
            }
            var tag = _mapper.Map<TagDto>(_tagRepository.GetTag(tagId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(tag);
        }

        [HttpGet("blog/tagId")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Blog>))]
        [ProducesResponseType(400)]
        public IActionResult GetBlogByTagId(Guid tagId)
        {
            var blogs = _mapper.Map<List<BlogDto>>(
                _tagRepository.GetBlogsByTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(blogs);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromBody] TagDto tagCreate)
        {
            if (tagCreate == null)
                return BadRequest(ModelState);
            var tag = _tagRepository.GetTags().Where(t => t.TagName.Trim().ToUpper() == tagCreate.TagName.Trim().ToUpper()).FirstOrDefault();
            if(tag !=null)
            {
                ModelState.AddModelError("", "Tag alredy exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var tagMap = _mapper.Map<Tag>(tagCreate);
            if(!_tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok(" Successfuly Created");
        }

        [HttpPut("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag(Guid tagId, [FromBody]TagDto updateTag) 
        {
            if (updateTag == null) return BadRequest(ModelState);

            if (tagId != updateTag.Id) return BadRequest(ModelState);

            if (!_tagRepository.TagExist(tagId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            var tagMap = _mapper.Map<Tag>(updateTag);

            if (!_tagRepository.UpdateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong updating tag");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(Guid tagId)
        {
            if (!_tagRepository.TagExist(tagId)) return NotFound();

            var tgToDelete = _tagRepository.GetTag(tagId);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (!_tagRepository.DeleteTag(tgToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting tag");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
