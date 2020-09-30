using System.Threading.Tasks;

namespace UnrealEstate.Infrastructure.Repository
{
    public abstract class BaseRepository<TEntity> where TEntity: class
    {
        private readonly UnrealEstateDbContext _context;

        public BaseRepository(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }
    }
}
