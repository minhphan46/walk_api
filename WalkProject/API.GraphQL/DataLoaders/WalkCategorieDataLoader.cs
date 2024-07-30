using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.DataLoaders
{
    public class WalkCategorieDataLoader : BatchDataLoader<Guid, List<WalkCategory>>
    {
        private readonly WalkCategoriesResolver _resolver;

        public WalkCategorieDataLoader(WalkCategoriesResolver walkCategoriesResolver, IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
            _resolver = walkCategoriesResolver;
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<WalkCategory>>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            IEnumerable<WalkCategory> walkCategories = await _resolver.GetWalkCategoriesByWalkId(keys);

            // Convert the IEnumerable<WalkCategory> to a dictionary with WalkId as the key
            var walkCategoriesDict = walkCategories
                .GroupBy(wc => wc.WalkId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return walkCategoriesDict;
        }
    }
}
