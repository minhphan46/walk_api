using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class WalksResolver
    {
        private readonly IDbContextFactory<NZWalksDbContext> _dbContextFactory;

        public WalksResolver(IDbContextFactory<NZWalksDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Walk> CreateAsync(Walk walk, Guid categoryId)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var categoryEntity = context.Categories.Where(t => t.Id == categoryId).FirstOrDefault();

                if (categoryEntity == null)
                {
                    return null;
                }

                var walkCategory = new WalkCategory()
                {
                    CategoryId = categoryId,
                    WalkId = walk.Id,
                    Category = categoryEntity,
                    Walk = walk
                };

                await context.WalkCategories.AddAsync(walkCategory);

                await context.Walks.AddAsync(walk);
                await context.SaveChangesAsync();
                return walk;
            }
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);

                if (existingWalk == null)
                {
                    return null;
                }
                // remove all walk categorys associated with this walk
                var walkCategories = context.WalkCategories.Where(wt => wt.WalkId == id).ToList();
                context.WalkCategories.RemoveRange(walkCategories);

                context.Walks.Remove(existingWalk);
                await context.SaveChangesAsync();
                return existingWalk;
            }
        }

        public async Task<List<Walk>> GetAllAsync(string filterOn = null, string filterQuery = null,
            string sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var walks = context.Walks.Include("Difficulty").Include("Region");

                // Filtering
                if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
                {
                    if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = walks.Where(x => x.Name.Contains(filterQuery));
                    }
                }

                // Sorting 
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                    }
                    else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                    {
                        walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                    }
                }

                // Pagination
                var skipResults = (pageNumber - 1) * pageSize;

                return await walks
                    .Skip(skipResults)
                    .Take(pageSize)
                    .Include(w => w.WalkCategories)
                    .ThenInclude(wt => wt.Category)
                    .ToListAsync();
                //return await context.Walks.Include("Difficulty").Include("Region").ToListAsync();
            }
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Walks
                    //.Include(w => w.WalkCategories)
                    //.ThenInclude(wt => wt.Category)
                    .ToListAsync();
            }
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .Include(w => w.WalkCategories)
                .ThenInclude(wt => wt.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingWalk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);

                if (existingWalk == null)
                {
                    return null;
                }


                existingWalk.Name = walk.Name;
                existingWalk.Description = walk.Description;
                existingWalk.LengthInKm = walk.LengthInKm;
                existingWalk.WalkImageUrl = walk.WalkImageUrl;
                existingWalk.DifficultyId = walk.DifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await context.SaveChangesAsync();

                return existingWalk;
            }
        }
    }
}
