using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Models;

namespace UnrealEstate.Services
{
    public interface ICommentService
    {
        Task CreateCommentAsync(Comment comment);
        Task EditCommentAsync(string currentUserId, Comment comment);
        Task<Comment> GetCommentAsync(int commentId);
        Task<List<Comment>> GetCommentsByListingAsync(int commentId);
    }
}