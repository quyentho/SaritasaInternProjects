using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;

namespace UnrealEstate.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository repository)
        {
            _commentRepository = repository;
        }

        /// <summary>
        /// Gets list of comments.
        /// </summary>
        /// <returns>List of comment</returns>
        public Task<List<Comment>> GetCommentsByListingAsync(int commentId)
            => _commentRepository.GetAllByListingAsync(commentId);

        public Task<Comment> GetCommentAsync(int commentId)
            => _commentRepository.GetCommentByIdAsync(commentId);

        public async Task CreateCommentAsync(Comment comment) 
            => await _commentRepository.AddCommentAsync(comment);

        public async Task EditCommentAsync(string currentUserId ,Comment comment)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(comment.Id);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            await _commentRepository.UpdateCommentAsync(comment);
        }
    }
}
