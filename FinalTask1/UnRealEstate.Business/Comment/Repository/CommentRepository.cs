using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Infrastructure;
using UnrealEstate.Infrastructure.Repository;

namespace UnrealEstate.Business.Comment.Repository
{
    public class CommentRepository : BaseRepository<Infrastructure.Models.Comment>, ICommentRepository
    {
        private readonly UnrealEstateDbContext _context;

        public CommentRepository(UnrealEstateDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Infrastructure.Models.Comment>> GetAllByListingAsync(int listingId)
        {
            return _context.Comments.Where(c => c.ListingId == listingId).ToListAsync();
        }

        public async Task DeleteCommentAsync(Infrastructure.Models.Comment comment)
        {
            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }

        public async Task<Infrastructure.Models.Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}