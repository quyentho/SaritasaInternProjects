using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;

namespace UnrealEstate.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public CommentService(ICommentRepository repository, UserManager<User> userManager, IMapper mapper)
        {
            _commentRepository = repository;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<CommentResponseViewModel>> GetCommentsByListingAsync(int listingId)
        {
            var comments = await _commentRepository.GetAllByListingAsync(listingId);

            List<CommentResponseViewModel> commentViewModels = _mapper.Map<List<CommentResponseViewModel>>(comments);

            return commentViewModels;
        }

        /// <inheritdoc/>
        public async Task<CommentResponseViewModel> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            var commentViewModel = _mapper.Map<CommentResponseViewModel>(comment);

            return commentViewModel;
        }

        /// <inheritdoc/>
        public async Task CreateCommentAsync(string userId, CommentRequestViewModel commentViewModel)
        {
            var comment = _mapper.Map<Comment>(commentViewModel);
            
            comment.UserId = userId;
            comment.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.AddCommentAsync(comment);
        }

        /// <inheritdoc/>
        public async Task EditCommentAsync(string currentUserId, CommentRequestViewModel commentViewModel, int commentId)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(commentId);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            _mapper.Map(commentViewModel, commentFromDb);

            commentFromDb.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.UpdateCommentAsync(commentFromDb);
        }

        /// <inheritdoc/>
        public async Task DeleteCommentAsync(User currentUser, int commentId)
        {
            IList<string> userRole = await _userManager.GetRolesAsync(currentUser);

            GuardClauses.IsAdmin(userRole.First());

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}
