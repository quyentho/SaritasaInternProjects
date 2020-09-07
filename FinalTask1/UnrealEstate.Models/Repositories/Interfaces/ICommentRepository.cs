using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetAllByListingAsync(int listingId);
        Task UpdateCommentAsync(Comment comment);
    }
}