using AutoMapper;
using Facebook.DTO;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;
        public PostController(IPostRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
  
        // POST api/<PostController>
        [HttpPost]
        public async Task Post([FromForm] PostManipulationDTO postDTO)
        {
            await _repository.AddPost(_mapper.Map<Post>(postDTO));
        }

        [HttpGet("friendsPosts/{userId}")] //all friends posts
        public async Task<IActionResult> GetFriendsPosts(int userId)
        {
            try
            {
                var friendsPosts = await _repository.GetFriendsPosts(userId);

              

                return Ok(friendsPosts);

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("delete/{postId}")]
        public async Task<ActionResult> DeletePost(int postId)
        {
            var deleted = await _repository.DeletePostAsync(postId);

            if (deleted)
            {
                return Ok("Post deleted successfully.");
            }
            else
            {
                return NotFound("Post not found.");
            }
        }

        [HttpPut("updatePost/{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromForm] PostManipulationDTO updatedPost)
        {
            try
            {
                // Prvo proverite da li post sa datim ID-om postoji
                var response = await _repository.UpdatePostAsync(postId,_mapper.Map<Post>(updatedPost));
                if (response == false)
                {
                    return BadRequest("Post not found");
                }
                return Ok("Post Updated succesfull");
         

            }
            catch (Exception ex)
            {
                // Obrada izuzetaka
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
