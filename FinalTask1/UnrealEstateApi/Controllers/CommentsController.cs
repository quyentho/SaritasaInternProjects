﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Services;

namespace UnrealEstateApi.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IAuthenticationService _userService;

        public CommentsController(ICommentService commentService, IAuthenticationService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        /// <summary>
        /// Create new comment.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update comment, only available for comment's author.
        /// </summary>
        /// <param name="commentId">comment id.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                User currentUser = await GetCurrentUser();

                await _commentService.EditCommentAsync(currentUser.Id, commentId);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
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
                User currentUser = await GetCurrentUser();

                var comment = await _commentService.GetCommentAsync(commentId);

                await _commentService.DeleteCommentAsync(currentUser, comment);
            }
            catch (NotSupportedException ex)
            {
                return Forbid(ex.Message);
            }

            return Ok();
        }

        private async Task<User> GetCurrentUser()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            User currentUser = await _userService.GetUserByEmailAsync(email);

            return currentUser;
        }
    }
}
