﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnrealEstate.Business.Comment.Repository
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Infrastructure.Models.Comment comment);
        Task DeleteCommentAsync(Infrastructure.Models.Comment comment);
        Task<Infrastructure.Models.Comment> GetCommentByIdAsync(int id);
        Task<List<Infrastructure.Models.Comment>> GetAllByListingAsync(int listingId);
        Task UpdateCommentAsync(Infrastructure.Models.Comment comment);
    }
}