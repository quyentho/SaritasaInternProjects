using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnrealEstate.Services.Comment.Repository.Interface
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Models.Models.Comment comment);
        Task DeleteCommentAsync(Models.Models.Comment comment);
        Task<Models.Models.Comment> GetCommentByIdAsync(int id);
        Task<List<Models.Models.Comment>> GetAllByListingAsync(int listingId);
        Task UpdateCommentAsync(Models.Models.Comment comment);
    }
}