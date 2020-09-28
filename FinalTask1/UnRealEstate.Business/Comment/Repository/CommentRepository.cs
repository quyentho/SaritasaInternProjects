using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnrealEstate.Business.Comment.Repository.Interface;
using UnrealEstate.Infrastructure;

namespace UnrealEstate.Business.Comment.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly UnrealEstateDbContext _context;

        public CommentRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public Task<List<Infrastructure.Models.Comment>> GetAllByListingAsync(int listingId)
        {
            return _context.Comments.Where(c => c.ListingId == listingId).ToListAsync();
        }

        public async Task AddCommentAsync(Infrastructure.Models.Comment comment)
        {
            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Infrastructure.Models.Comment comment)
        {
            _context.Entry(comment).CurrentValues.SetValues(comment);

            await _context.SaveChangesAsync();
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