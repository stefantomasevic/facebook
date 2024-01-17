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
    public class CommentController : ControllerBase
    {

        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        public CommentController(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // GET: api/<CommentController>
   
        [HttpGet("comments/{postId}")]
        public async Task<List<CommentsOfPostDTO>> GetCommentsByPostId(int postId)
        {
            var comments = await _repository.GetAllCommentsAsync(postId);
           return (_mapper.Map<List<CommentsOfPostDTO>>(comments));
        }

        [HttpPost("add")]
        public async Task<ActionResult<CommentsOfPostDTO>> AddComment([FromBody] CommentsOfPostDTO commentDTO)
        {
            var commentToAdd = _mapper.Map<Comment>(commentDTO);
            var addedComment = await _repository.AddCommentAsync(commentToAdd);

            var commentDtoResult = _mapper.Map<CommentsOfPostDTO>(addedComment);

            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = commentDtoResult.postId }, commentDtoResult);
        }

        [HttpPut("edit/{commentId}")]
        public async Task<ActionResult> EditComment(int commentId, string editedComment)
        {
            try
            {
                var result = await _repository.EditCommentAsync(commentId, editedComment);

                // Check if the result is a success message
                if (result == "Comment updated successfully")
                {
                    return Ok("Comment updated successfully");
                }
                else
                {
                    // If the result is not a success message, assume it's an error message
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");

            }

        }

        [HttpDelete("delete/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var isDeleted = await _repository.DeleteCommentAsync(commentId);

            if (!isDeleted)
                return NotFound(); 

            return NoContent(); 
        }


    }
}
