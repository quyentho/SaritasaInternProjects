using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Route("api/comments")]
        public async Task<IActionResult> CreateNewComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _commentService.CreateCommentAsync(comment);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [Route("api/comments/{id}")]
        public async Task<IActionResult> UpdateComment(int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User currentUser = 
            _commentService.EditCommentAsync()
        }
    }
}
