using WalkProject.DataModels.Entities;

namespace NZWalks.GraphQL.DataLoaders
{
    public class WalkDataLoader : BatchDataLoader<Guid, Walk>
    {
        public WalkDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
        }

        protected override Task<IReadOnlyDictionary<Guid, Walk>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
