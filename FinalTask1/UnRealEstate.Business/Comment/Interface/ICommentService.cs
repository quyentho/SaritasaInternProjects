using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Business.Comment.ViewModel.Request;
using UnrealEstate.Business.Comment.ViewModel.Response;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Business.Comment.Interface
{
    public interface ICommentService
    {
        /// <summary>
        ///     Creates new comment.
        /// </summary>
        /// <param name="comment">new comment.</param>
        /// <returns></returns>
        Task CreateCommentAsync(string userId, CommentRequest comment);

        /// <summary>
        ///     Edits comment, only available for author.
        /// </summary>
        /// <param name="currentUserId">current logged in user id.</param>
        /// <param name="comment">comment edited.</param>
        /// <returns></returns>
        Task EditCommentAsync(string currentUserId, CommentRequest commentViewModel, int commentId);

        /// <summary>
        ///     Gets comment by id.
        /// </summary>
        /// <param name="commentId">comment id.</param>
        /// <returns>comment found.</returns>
        Task<CommentResponse> GetCommentAsync(int commentId);

        /// <summary>
        ///     Gets all comment in listing.
        /// </summary>
        /// <param name="listingId">listing id.</param>
        /// <returns>List comments found.</returns>
        Task<List<CommentResponse>> GetCommentsByListingAsync(int listingId);

        /// <summary>
        ///     Deletes comment by by comment id.
        /// </summary>
        /// <returns></returns>
        Task DeleteCommentAsync(ApplicationUser currentUser, int commentId);
    }
}