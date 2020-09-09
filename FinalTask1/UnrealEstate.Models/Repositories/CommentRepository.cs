using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly UnrealEstateDbContext _context;

        public CommentRepository(UnrealEstateDbContext context)
        {
            this._context = context;
        }

        public Task<List<Comment>> GetAllByListingAsync(int listingId)
        {
            return _context.Comments.Where(c => c.ListingId == listingId).ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Entry<Comment>(comment).CurrentValues.SetValues(comment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}
