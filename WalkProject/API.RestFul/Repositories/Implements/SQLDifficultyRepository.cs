using Microsoft.EntityFrameworkCore;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.RestFul.Repositories.Implements
{
    public class SQLDifficultyRepository : IDifficultyRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLDifficultyRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Difficulty> CreateAsync(Difficulty difficulty)
        {
            await dbContext.Difficulties.AddAsync(difficulty);
            await dbContext.SaveChangesAsync();
            return difficulty;
        }

        public async Task<Difficulty> DeleteAsync(Guid id)
        {
            var existingDifficulty = await dbContext.Difficulties.FirstOrDefaultAsync(x => x.Id == id);

            if (existingDifficulty == null)
            {
                return null;
            }

            dbContext.Difficulties.Remove(existingDifficulty);
            await dbContext.SaveChangesAsync();
            return existingDifficulty;
        }

        public async Task<List<Difficulty>> GetAllAsync()
        {
            return await dbContext.Difficulties.ToListAsync();
        }

        public async Task<Difficulty> GetByIdAsync(Guid id)
        {
            return await dbContext.Difficulties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Difficulty> UpdateAsync(Guid id, Difficulty difficulty)
        {
            var existingDifficulty = await dbContext.Difficulties.FirstOrDefaultAsync(x => x.Id == id);

            if (existingDifficulty == null)
            {
                return null;
            }

            existingDifficulty.Name = difficulty.Name;

            await dbContext.SaveChangesAsync();
            return existingDifficulty;
        }
    }
}
