using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UnrealEstate.Business.Comment.Repository;
using UnrealEstate.Business.Comment.Service;
using UnrealEstate.Business.Comment.ViewModel;
using UnrealEstate.Business.Listing.Repository;
using UnrealEstate.Business.Utils;
using UnrealEstate.Infrastructure.Models;

namespace UnrealEstate.Business.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IListingRepository _listingRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(ICommentRepository repository, UserManager<ApplicationUser> userManager, IMapper mapper,
            IListingRepository listingRepository)
        {
            _commentRepository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _listingRepository = listingRepository;
        }

        /// <inheritdoc />
        public async Task<List<CommentResponse>> GetCommentsByListingAsync(int listingId)
        {
            var comments = await _commentRepository.GetAllByListingAsync(listingId);

            var commentViewModels = _mapper.Map<List<CommentResponse>>(comments);

            return commentViewModels;
        }

        /// <inheritdoc />
        public async Task<CommentResponse> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            var commentViewModel = _mapper.Map<CommentResponse>(comment);

            return commentViewModel;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">listing does not exist in database.</exception>
        /// <exception cref="InvalidOperationException">Listing status is not allowed for adding comment</exception>
        public async Task CreateCommentAsync(string userId, CommentRequest commentViewModel)
        {
            var listing = await _listingRepository.GetListingByIdAsync(commentViewModel.ListingId);

            GuardClauses.HasValue(listing, "listing id");
            GuardClauses.IsAllowCommentStatus(listing.StatusId);

            var comment = _mapper.Map<Infrastructure.Models.Comment>(commentViewModel);

            comment.UserId = userId;
            comment.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.AddAsync(comment);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">commentId does not exist in database.</exception>
        /// <exception cref="NotSupportedException">current user is not the author of this comment, so cannot edit the comment.</exception>
        public async Task EditCommentAsync(string currentUserId, CommentRequest commentViewModel, int commentId)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(commentId);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            _mapper.Map(commentViewModel, commentFromDb);

            commentFromDb.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.UpdateAsync(commentFromDb);
        }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">current user is not an admin, so cannot delete the comment.</exception>
        public async Task DeleteCommentAsync(ApplicationUser currentUser, int commentId)
        {
            var userRole = await _userManager.GetRolesAsync(currentUser);

            GuardClauses.IsAdmin(userRole.First());

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            GuardClauses.HasValue(comment, "comment id");

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}