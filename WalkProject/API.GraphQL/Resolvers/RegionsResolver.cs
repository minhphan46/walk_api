using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class RegionsResolver
    {
        private readonly IDbContextFactory<NZWalksDbContext> _dbContextFactory;

        public RegionsResolver(IDbContextFactory<NZWalksDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                await context.Regions.AddAsync(region);
                await context.SaveChangesAsync();
                return region;
            }
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingRegion = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

                if (existingRegion == null)
                {
                    return null;
                }

                context.Regions.Remove(existingRegion);
                await context.SaveChangesAsync();
                return existingRegion;
            }
        }

        public async Task<List<Region>> GetAllAsync()
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Regions.ToListAsync();
            }
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Regions.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingRegion = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

                if (existingRegion == null)
                {
                    return null;
                }

                existingRegion.Code = region.Code;
                existingRegion.Name = region.Name;
                existingRegion.RegionImageUrl = region.RegionImageUrl;

                await context.SaveChangesAsync();
                return existingRegion;
            }
        }

        public async Task<IEnumerable<Region>> GetRegionsByWalkId(IReadOnlyList<Guid> regionIds)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Regions.Where(r => regionIds.Contains(r.Id)).ToListAsync();
            }
        }
    }
}
