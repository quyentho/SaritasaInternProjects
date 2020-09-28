﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Infrastructure.Models;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.ViewModels;
using UnrealEstate.Models.ViewModels.RequestViewModels;
using UnrealEstate.Models.ViewModels.ResponseViewModels;
using UnrealEstate.Services.Comment.Repository.Interface;

namespace UnrealEstate.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IListingRepository _listingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public CommentService(ICommentRepository repository, UserManager<ApplicationUser> userManager, IMapper mapper, IListingRepository listingRepository)
        {
            _commentRepository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _listingRepository = listingRepository;
        }

        /// <inheritdoc/>
        public async Task<List<CommentResponse>> GetCommentsByListingAsync(int listingId)
        {
            var comments = await _commentRepository.GetAllByListingAsync(listingId);

            List<CommentResponse> commentViewModels = _mapper.Map<List<CommentResponse>>(comments);

            return commentViewModels;
        }

        /// <inheritdoc/>
        public async Task<CommentResponse> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            var commentViewModel = _mapper.Map<CommentResponse>(comment);

            return commentViewModel;
        }

        /// <inheritdoc/>
        public async Task CreateCommentAsync(string userId, CommentRequest commentViewModel)
        {
            var listing = await _listingRepository.GetListingByIdAsync(commentViewModel.ListingId);
            
            GuardClauses.HasValue(listing, "listing id");
            GuardClauses.IsAllowCommentStatus(listing.StatusId);

            var comment = _mapper.Map<Infrastructure.Models.Comment>(commentViewModel);
            
            comment.UserId = userId;
            comment.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.AddCommentAsync(comment);
        }

        /// <inheritdoc/>
        public async Task EditCommentAsync(string currentUserId, CommentRequest commentViewModel, int commentId)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(commentId);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            _mapper.Map(commentViewModel, commentFromDb);

            commentFromDb.CreatedAt = DateTimeOffset.Now;

            await _commentRepository.UpdateCommentAsync(commentFromDb);
        }

        /// <inheritdoc/>
        public async Task DeleteCommentAsync(ApplicationUser currentUser, int commentId)
        {
            IList<string> userRole = await _userManager.GetRolesAsync(currentUser);

            GuardClauses.IsAdmin(userRole.First());

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            
            GuardClauses.HasValue(comment, "comment id");

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}
