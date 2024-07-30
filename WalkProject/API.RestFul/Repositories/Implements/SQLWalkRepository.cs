using Microsoft.EntityFrameworkCore;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.RestFul.Repositories.Implements
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Walk> CreateAsync(Walk walk, Guid categoryId)
        {
            var categoryEntity = dbContext.Categories.Where(t => t.Id == categoryId).FirstOrDefault();

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

            await dbContext.WalkCategories.AddAsync(walkCategory);

            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }
            // remove all walk categorys associated with this walk
            var walkCategories = dbContext.WalkCategories.Where(wt => wt.WalkId == id).ToList();
            dbContext.WalkCategories.RemoveRange(walkCategories);

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string filterOn = null, string filterQuery = null,
            string sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region");

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
            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .Include(w => w.WalkCategories)
                .ThenInclude(wt => wt.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

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

            await dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}