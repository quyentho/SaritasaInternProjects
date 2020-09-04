using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly UnrealEstateDbContext _context;

        public BaseRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entityFromDb = _context.Set<TEntity>().Find(id);

            _ = entityFromDb ?? throw new ArgumentOutOfRangeException();

            _context.Set<TEntity>().Remove(entityFromDb);

            await _context.SaveChangesAsync();
        }

        public Task<TEntity> GetAsync(int id)
        {
            return _context.Set<TEntity>().FindAsync(id).AsTask();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _context.Set<TEntity>().ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
