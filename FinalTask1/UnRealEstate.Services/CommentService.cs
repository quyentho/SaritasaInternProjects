using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnrealEstate.Models;
using UnrealEstate.Models.Repositories;
using UnrealEstate.Models.ViewModels;

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
        public async Task<List<CommentViewModel>> GetCommentsByListingAsync(int listingId)
        {
            var comments =  await _commentRepository.GetAllByListingAsync(listingId);

            List<CommentViewModel> commentViewModels = _mapper.Map<List<CommentViewModel>>(comments);

            return commentViewModels;
        }

        /// <inheritdoc/>
        public async Task<CommentViewModel> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            var commentViewModel = _mapper.Map<CommentViewModel>(comment);

            return commentViewModel;
        }

        /// <inheritdoc/>
        public async Task CreateCommentAsync(CommentViewModel commentViewModel)
        {
            var comment = _mapper.Map<Comment>(commentViewModel);

            await _commentRepository.AddCommentAsync(comment);
        }

        /// <inheritdoc/>
        public async Task EditCommentAsync(string currentUserId, CommentViewModel commentViewModel)
        {
            var commentFromDb = await _commentRepository.GetCommentByIdAsync(commentViewModel.Id);

            GuardClauses.HasValue(commentFromDb, "comment id");

            GuardClauses.IsAuthor(currentUserId, commentFromDb.UserId);

            _mapper.Map(commentViewModel, commentFromDb);

            await _commentRepository.UpdateCommentAsync(commentFromDb);
        }

        public async Task DeleteCommentAsync(User currentUser, int commentId)
        {
            IList<string> userRole = await _userManager.GetRolesAsync(currentUser);

            GuardClauses.IsAdmin(userRole.First());

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            await _commentRepository.DeleteCommentAsync(comment);
        }
    }
}
