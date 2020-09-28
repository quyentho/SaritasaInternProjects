using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Models;
using UnrealEstate.Services.Comment.Repository.Interface;

namespace UnrealEstate.Services.Comment.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly UnrealEstateDbContext _context;

        public CommentRepository(UnrealEstateDbContext context)
        {
            this._context = context;
        }

        public Task<List<Models.Models.Comment>> GetAllByListingAsync(int listingId)
        {
            return _context.Comments.Where(c => c.ListingId == listingId).ToListAsync();
        }

        public async Task AddCommentAsync(Models.Models.Comment comment)
        {
            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Models.Models.Comment comment)
        {
            _context.Entry<Models.Models.Comment>(comment).CurrentValues.SetValues(comment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Models.Models.Comment comment)
        {
            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }

        public async Task<Models.Models.Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}
