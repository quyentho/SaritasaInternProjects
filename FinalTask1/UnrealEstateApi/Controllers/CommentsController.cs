using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentsController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        /// <summary>
        /// Create new comment.
        /// </summary>
        /// <param name="commentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/comments")]
        public async Task<IActionResult> CreateNewComment(CommentRequest commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User currentUser = await GetCurrentUserAsync();

            try
            {
                await _commentService.CreateCommentAsync(currentUser.Id, commentViewModel);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Update comment, only available for comment's author.
        /// </summary>
        /// <param name="commentViewModel">comment id.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, CommentRequest commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                User currentUser = await GetCurrentUserAsync();

                await _commentService.EditCommentAsync(currentUser.Email, commentViewModel, commentId);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return Forbid(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete comment by id, only available for admin.
        /// </summary>
        /// <param name="commentId">comment id.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                User currentUser = await GetCurrentUserAsync();

                await _commentService.DeleteCommentAsync(currentUser, commentId);
            }
            catch (NotSupportedException)
            {
                return Forbid();
            }

            return Ok();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            User currentUser = await _userService.GetUserByEmailAsync(email);

            return currentUser;
        }
    }
}
