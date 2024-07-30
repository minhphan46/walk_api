using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.DataLoaders
{
    public class DifficultyDataLoader : BatchDataLoader<Guid, Difficulty>
    {
        private readonly DifficultiesResolver _resolver;

        public DifficultyDataLoader(DifficultiesResolver difficultiesResolver, IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
            _resolver = difficultiesResolver;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Difficulty>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            IEnumerable<Difficulty> difficulties = await _resolver.GetDifficultiesByWalkId(keys);

            return difficulties.ToDictionary(i => i.Id);
        }
    }
}
