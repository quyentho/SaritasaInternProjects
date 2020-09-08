using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
namespace UnrealEstate.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;

        public CommentService(ICommentRepository repository, UserManager<User> userManager)
        {
            _commentRepository = repository;
            _userManager = userManager;
        }

        /// <inheritdoc/>
        public Task<List<Comment>> GetCommentsByListingAsync(int listingId)
        {
            return _commentRepository.GetAllByListingAsync(listingId);
        }

        /// <inheritdoc/>
        public Task<Comment> GetCommentAsync(int commentId)
        {
            return _commentRepository.GetCommentByIdAsync(commentId);
        }

        /// <inheritdoc/>
        public async Task CreateCommentAsync(Comment comment)
        {
            await _commentRepository.AddCommentAsync(comment);
        }

        /// <inheritdoc/>
        public async Task EditCommentAsync(string currentUserId, int commentId)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(commentId);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            await _commentRepository.UpdateCommentAsync(commentFromDb);
        }

        public async Task DeleteCommentAsync(User currentUser, Comment comment)
        {
            IList<string> userRole = await _userManager.GetRolesAsync(currentUser);

            GuardClauses.IsAdmin(userRole.First());

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}
