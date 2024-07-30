using Microsoft.EntityFrameworkCore;
using WalkProject.DataModels.DbContexts;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class WalkCategoriesResolver
    {
        private readonly IDbContextFactory<NZWalksDbContext> _dbContextFactory;

        public WalkCategoriesResolver(IDbContextFactory<NZWalksDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<WalkCategory>> GetWalkCategoriesByWalkId(IReadOnlyList<Guid> walkIds)
        {
            using (NZWalksDbContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.WalkCategories.Where(d => walkIds.Contains(d.WalkId)).Include(x => x.Category).ToListAsync();
            }
        }
    }
}
