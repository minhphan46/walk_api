using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class CategoriesResolver
    {
        private readonly IDbContextFactory<NZWalksDbContext> _dbContextFactory;

        public CategoriesResolver(IDbContextFactory<NZWalksDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return category;
            }
        }

        public async Task<Category> DeleteAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (existingCategory == null)
                {
                    return null;
                }

                // remove all walk categorys associated with this category
                var walkCategories = context.WalkCategories.Where(wt => wt.WalkId == id).ToList();
                context.WalkCategories.RemoveRange(walkCategories);

                context.Categories.Remove(existingCategory);
                await context.SaveChangesAsync();
                return existingCategory;
            }
        }

        public async Task<List<Category>> GetAllAsync()
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Categories.ToListAsync();
            }
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Category> UpdateAsync(Guid id, Category category)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (existingCategory == null)
                {
                    return null;
                }

                existingCategory.Name = category.Name;

                await context.SaveChangesAsync();
                return existingCategory;
            }
        }
    }
}
